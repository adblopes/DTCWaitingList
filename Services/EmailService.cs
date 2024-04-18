using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Text;
using MimeKit;
using System.IO;
using DTCWaitingList.Interface;
using DTCWaitingList.Views;
using DTCWaitingList.Models;
using Google.Apis.Util;

namespace DTCWaitingList.Services
{
    public class EmailService : IEmailService
    {
        // client configuration
        const string clientID = "59857479736-1v8hl51hd8029m0v10jsah5la74sle9o.apps.googleusercontent.com";
        const string clientSecret = "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";
        const string hostEmail = "adlopesrepo@gmail.com";

        private readonly IDataAccessService _data;
        public GmailService _service { get; set; }

        public EmailService(IDataAccessService data, GmailService service)
        {
            _data = data;
            _service = service;
        }

        public void SendEmail(string userEmail, string userName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dental Treatment Center", hostEmail));
            message.To.Add(new MailboxAddress("", userEmail));
            message.Subject = "Waiting List Confirmation - DO NOT REPLY";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Dear {userName}\n\n We confirm your request to join our waiting list, " +
                $"you will be notified of any vacancies according to your submitted timeslots and position in the waiting list.\n\n Thank you and kind regards.\n\n" +
                $"Dental Treatment Center\n235 Rue de la Loi Bruxelles, 1040\n\nTHIS IS AN AUTOMATED SERVICE - PLEASE DO NOT REPLY TO THIS EMAIL";

            message.Body = bodyBuilder.ToMessageBody();

            var rawMessage = ConvertToRaw(message);

            var gmailMessage = new Message { Raw = rawMessage };

            _service.Users.Messages.Send(gmailMessage, hostEmail).Execute();

        }

        public AppointmentView ReadEmail(string messageId)
        {
            AppointmentView appointment = new();

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

                appointment.FullName = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Name) + emailParams.Name.Length, decodedMessage.IndexOf(emailParams.Email) - decodedMessage.IndexOf(emailParams.Name) - emailParams.Name.Length).Trim();
                appointment.Email = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Email) + emailParams.Email.Length, decodedMessage.IndexOf(emailParams.Phone) - decodedMessage.IndexOf(emailParams.Email) - emailParams.Email.Length).Trim();
                appointment.Phone = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Phone) + emailParams.Phone.Length, decodedMessage.IndexOf("Are you") - decodedMessage.IndexOf(emailParams.Phone) - emailParams.Phone.Length).Trim();
                appointment.IsClient = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Current) + emailParams.Current.Length, decodedMessage.IndexOf(emailParams.Days) - decodedMessage.IndexOf(emailParams.Current) - emailParams.Current.Length).Trim() == "Yes";
                appointment.FullReason = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Comment) + emailParams.Comment.Length, decodedMessage.IndexOf("SID:") - decodedMessage.IndexOf(emailParams.Comment) - emailParams.Comment.Length).Trim();
                appointment.AvailableDays = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Days) + emailParams.Days.Length, decodedMessage.IndexOf(emailParams.Times) - decodedMessage.IndexOf(emailParams.Days) - emailParams.Days.Length).Trim().Replace("\r", "").Split("\n");
                appointment.AvailableTimes = decodedMessage.Substring(decodedMessage.IndexOf(emailParams.Times) + emailParams.Times.Length, decodedMessage.IndexOf(emailParams.Comment) - decodedMessage.IndexOf(emailParams.Times) - emailParams.Times.Length).Trim().Replace("\r", "").Split("\n");

                var gmailDate = message.InternalDate ?? DateTimeOffset.Now.ToUnixTimeMilliseconds();
                appointment.CreatedDate = DateTimeOffset.FromUnixTimeMilliseconds(gmailDate).DateTime;

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't read email message, please call support. Error: {ex.Message}");
            }

        }

        public async Task ProcessInboxUnread()
        {
            _service = AuthenticateGmailService();

            var messages = ListMessages(hostEmail, "subject:(ProSites Appointment Request Response) is:unread");

            foreach (var message in messages)
            {
                var appointment = ReadEmail(message.Id);

                await _data.AddAppointment(appointment);

                SendEmail("adiogo.blopes@gmail.com", "test");  // <------ appointment.Email

                DeleteEmail(message.Id);
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
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
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
                new[] { GmailService.Scope.GmailModify },
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

        //private DateTime ParseDate(string emailDate)
        //{
        //    return DateTime.Now;
        //}

        private void DeleteEmail(string messageId)
        {
            _service.Users.Messages.Delete(hostEmail, messageId).Execute();
        }
    }
}
