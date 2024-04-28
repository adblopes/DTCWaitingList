using DTCWaitingList.Models;
using Google.Apis.Gmail.v1.Data;

namespace DTCWaitingList.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string userEmail, string userName);

        PatientView ReadEmail(string messageId);

        Task ProcessInboxUnreadAsync();

        List<Message> ListMessages(string userId, string query);
    }
}
