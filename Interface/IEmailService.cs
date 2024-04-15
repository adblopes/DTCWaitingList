using DTCWaitingList.Models;
using Google.Apis.Gmail.v1.Data;

namespace DTCWaitingList.Interface
{
    public interface IEmailService
    {
        void SendEmail(string userEmail, string userName);

        Appointment ReadEmail(string messageId);

        void ProcessInboxUnread();

        List<Message> ListMessages(string userId, string query);
    }
}
