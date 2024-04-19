using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Text;
using MimeKit;
using System.IO;
using DTCWaitingList.Interface;
using DTCWaitingList.Models;

namespace DTCWaitingList.Services
{
    public class EmailService : IEmailService
    {
        // client configuration
        const string clientID = "59857479736-1v8hl51hd8029m0v10jsah5la74sle9o.apps.googleusercontent.com";
        const string clientSecret = "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";
        const string hostEmail = "adlopesrepo@gmail.com";

        private readonly AppointmentsDbContext _dbContext;

        public GmailService Service { get; set; }

        public EmailService(AppointmentsDbContext dbContext, GmailService service)
        {
            _dbContext = dbContext;
            Service = service;
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

            Service.Users.Messages.Send(gmailMessage, hostEmail).Execute();

        }

        public Appointment ReadEmail(string messageId)
        {
            Appointment appointment = new Appointment();

            Message message = Service.Users.Messages.Get(hostEmail, messageId).Execute();

            var data = Convert.FromBase64String(message.Payload.Parts[0].Body.Data);

            var decodedMessage = Encoding.UTF8.GetString(data);

            appointment.FullName = decodedMessage.Substring(decodedMessage.IndexOf("Name:"), decodedMessage.IndexOf("Email:")).Trim();
            appointment.Email = decodedMessage.Substring(decodedMessage.IndexOf("Email:"), decodedMessage.IndexOf("Phone:")).Trim();
            appointment.Phone = decodedMessage.Substring(decodedMessage.IndexOf("Phone:"), decodedMessage.IndexOf("Are you")).Trim();
            appointment.IsClient = decodedMessage.Substring(decodedMessage.IndexOf("Are you a current patient?"), decodedMessage.IndexOf("Preferred day(s) of the week for an appointment?")).Trim() == "Yes";
            appointment.AvailableDays = decodedMessage.Substring(decodedMessage.IndexOf("Preferred day(s) of the week for an appointment?"), decodedMessage.IndexOf("Preferred time(s) for an appointment?")).Trim().Split("\n").ToList();
            appointment.AvailableTimes = decodedMessage.Substring(decodedMessage.IndexOf("Preferred time(s) for an appointment?"), decodedMessage.IndexOf("Comment:")).Trim().Split("\n").ToList();
            appointment.FullReason = decodedMessage.Substring(decodedMessage.IndexOf("Comment:"), decodedMessage.IndexOf("SID:")).Trim();
            appointment.Reasons = SearchReasons(appointment.FullReason);

            return appointment;
        }

        public void ProcessInboxUnread()
        {
            Service = AuthenticateGmailService();

            var messages = ListMessages(hostEmail, "subject:(ProSites Appointment Request Response) is:unread");

            foreach (var message in messages)
            {
                var appointment = ReadEmail(message.Id);

                _dbContext.AddAppointment(appointment);

                SendEmail("adiogo.blopes@gmail.com", "test");  // <------ appointment.Email

                DeleteEmail(message.Id);
            }
        }

        private void DeleteEmail(string messageId)
        {
            Service.Users.Messages.Delete(hostEmail, messageId).Execute();
        }

        // List Gmail messages
        public List<Message> ListMessages(string userId, string query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = Service.Users.Messages.List(userId);
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

        private List<Reason> SearchReasons(string input)
        {
            var result = new List<Reason>();
            var reasons = _dbContext.GetReasons();

            input = input.ToLower();

            foreach (var reason in reasons)
            {
                string reasonStr = reason.ReasonName!.ToLower();

                if (input.Contains(reasonStr))
                {
                    result.Add(reason);
                }
            }

            return result;
        }
    }
}
