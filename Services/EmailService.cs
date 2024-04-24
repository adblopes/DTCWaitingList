using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Text;
using MimeKit;
using System.IO;
using DTCWaitingList.Interfaces;
using System.Reflection;
using MimeKit.Utils;
using DTCWaitingList.Models;

namespace DTCWaitingList.Services
{
    public class EmailService : IEmailService
    {
        // client configuration
        const string clientID = "59857479736-te69q045pk1mimtme66oruukj8msbk0h.apps.googleusercontent.com";
        const string clientSecret = "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";
        const string hostEmail = "adlopesrepo@gmail.com";

        private readonly IDataAccessService _data;

        public GmailService _service { get; set; }

        public EmailService(IDataAccessService data, GmailService service)
        {
            _data = data;
            _service = service;
        }

        public async Task SendEmailAsync(string userEmail, string userName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dental Treatment Center", hostEmail));
            message.To.Add(new MailboxAddress("", userEmail));
            message.Subject = "Waiting List Confirmation - DO NOT REPLY";

            try
            {
                var bodyBuilder = new BodyBuilder();
                string imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"Resources\thumbnail.jpg");
                var image = bodyBuilder.LinkedResources.Add(imagePath);
                image.ContentId = MimeUtils.GenerateMessageId();

                bodyBuilder.HtmlBody = $@"
                    <div style='font-size: 16px; font-family: Arial, sans-serif;'>
                        <p>Dear {userName},</p>
                        <br>
                        <p>We confirm your request to join our waiting list, you will be notified of any vacancies according to your submitted availability and position in the waiting list.</p>
                        <br>
                        <p>Thank you and kind regards.</p>
                        <img src='cid:{image.ContentId}' style='float: left; display: block; margin: 0 auto; width: 33%; height: auto;'>
                        <br>
                        <br>
                        <p style='font-size: 12px;'>Dental Treatment Center\n235 Rue de la Loi Bruxelles, 1040</p>
                        <p style='font-size: 12px;'>235 Rue de la Loi Bruxelles, 1040</p>
                        <p style='font-size: 12px;'>THIS IS AN AUTOMATED SERVICE - PLEASE DO NOT REPLY TO THIS EMAIL</p>
                    </div>";

                message.Body = bodyBuilder.ToMessageBody();
                var rawMessage = ConvertToRaw(message);
                var gmailMessage = new Message { Raw = rawMessage };

                await _service.Users.Messages.Send(gmailMessage, hostEmail).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to send message, please verify email credentials and try again. Error: {ex.Message}");
            }
        }

        public PatientView ReadEmail(string messageId)
        {
            PatientView patientView = new();

            try
            {
                Message message = _service.Users.Messages.Get(hostEmail, messageId).Execute();

                var data = Convert.FromBase64String(message.Payload.Parts[0].Body.Data);

                var decodedMessage = Encoding.UTF8.GetString(data);

                var emailParams = new
                {
                    Name = "Name:",
                    Email = "Email:",
                    Phone = "Phone:",
                    Current = "Are you a current patient?",
                    Comment = "Comment:",
                    Days = "Preferred day(s) of the week for an appointment?",
                    Times = "Preferred time(s) for an appointment?",
                };

                patientView.FullName = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Name) + emailParams.Name.Length, decodedMessage.IndexOf(emailParams.Email) - decodedMessage.IndexOf(emailParams.Name) - emailParams.Name.Length).Trim();
                patientView.Email = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Email) + emailParams.Email.Length, decodedMessage.IndexOf(emailParams.Phone) - decodedMessage.IndexOf(emailParams.Email) - emailParams.Email.Length).Trim();
                patientView.Phone = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Phone) + emailParams.Phone.Length, decodedMessage.IndexOf("Are you") - decodedMessage.IndexOf(emailParams.Phone) - emailParams.Phone.Length).Trim();
                patientView.IsClient = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Current) + emailParams.Current.Length, decodedMessage.IndexOf(emailParams.Days) - decodedMessage.IndexOf(emailParams.Current) - emailParams.Current.Length).Trim() == "Yes";
                patientView.FullReason = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Comment) + emailParams.Comment.Length, decodedMessage.IndexOf("SID:") - decodedMessage.IndexOf(emailParams.Comment) - emailParams.Comment.Length).Trim();

                var days = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Days) + emailParams.Days.Length, decodedMessage.IndexOf(emailParams.Times) - decodedMessage.IndexOf(emailParams.Days) - emailParams.Days.Length).Replace("\r\n", string.Empty).Trim();
                var times = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Times) + emailParams.Times.Length, decodedMessage.IndexOf(emailParams.Comment) - decodedMessage.IndexOf(emailParams.Times) - emailParams.Times.Length).Replace("\r\n", string.Empty).Trim();

                var tempDays = days.Contains("Any Day") ? [days] : days.Split(" ");
                var tempTimes = times.Contains("Any Day") ? [times] : times.Split(" ");

                patientView.PatientDays = new List<string>();
                patientView.PatientTimes = new List<string>();

                foreach (var day in tempDays)
                {
                    patientView.PatientDays.Add(day);
                }
                foreach (var time in tempTimes)
                {
                    patientView.PatientTimes.Add(time);
                }

                //if gmail doesn't return the date (in unix time milliseconds) just add today's date
                var gmailDate = message.InternalDate ?? DateTimeOffset.Now.ToUnixTimeMilliseconds();
                patientView.CreatedDate = DateTimeOffset.FromUnixTimeMilliseconds(gmailDate + (long)TimeSpan.FromMinutes(60).TotalMilliseconds).DateTime;

                return patientView;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't read email message, please call support. Error: {ex.Message}");
            }
        }

        public async Task ProcessInboxUnreadAsync()
        {
            _service = AuthenticateGmailService();

            var messages = ListMessages(hostEmail, "subject:(ProSites Appointment Request Response) is:unread");

            foreach (var message in messages)
            {
                var patientView = ReadEmail(message.Id);

                if (patientView != null)
                {
                    await _data.AddPatientAsync(patientView);
                    //await SendEmailAsync("adiogo.blopes@gmail.com", appointment.FullName!);  // <------ appointment.Email
                    //await DeleteEmailAsync(message.Id);
                }
            }
        }

        // List Gmail messages
        public List<Message> ListMessages(string userId, string query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = _service.Users.Messages.List(userId);
            request.Q = query;

            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();

                    if (response.Messages != null)
                    {
                        result.AddRange(response.Messages);
                        request.PageToken = response.NextPageToken;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!string.IsNullOrEmpty(request.PageToken));

            return result;
        }

        private GmailService AuthenticateGmailService()
        {
            // Create OAuth Credential.
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                [GmailService.Scope.GmailModify],
                hostEmail,
                CancellationToken.None).Result;

            // Create the service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DTC Waiting List",
            });

            return service;
        }

        // Helper method to convert a MimeMessage to its raw RFC 2822 representation
        private string ConvertToRaw(MimeMessage message)
        {
            using (var stream = new MemoryStream())
            {
                message.WriteTo(stream);
                return Convert.ToBase64String(stream.ToArray())
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");
            }
        }

        private async Task DeleteEmailAsync(string messageId)
        {
            // prefer Trash over Delete as it moves to bin and is of less invasive scope
            await _service.Users.Messages.Trash(hostEmail, messageId).ExecuteAsync();
        }
    }
}
