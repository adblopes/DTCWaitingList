using DTCWaitingList.Models;
using Google.Apis.Gmail.v1.Data;

namespace DTCWaitingList.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string userEmail, string userName);

        AppointmentView ReadEmail(string messageId);

        Task ProcessInboxUnreadAsync();

        List<Message> ListMessages(string userId, string query);
    }
}
