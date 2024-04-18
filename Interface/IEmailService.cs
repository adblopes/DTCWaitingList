using DTCWaitingList.Views;
using Google.Apis.Gmail.v1.Data;

namespace DTCWaitingList.Interface
{
    public interface IEmailService
    {
        void SendEmail(string userEmail, string userName);

        AppointmentView ReadEmail(string messageId);

        Task ProcessInboxUnread();

        List<Message> ListMessages(string userId, string query);
    }
}
