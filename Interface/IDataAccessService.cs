using DTCWaitingList.Models;
using DTCWaitingList.Views;

namespace DTCWaitingList.Interface
{
    public interface IDataAccessService
    {
        public List<AppointmentView> GetAppointments(string[]? args);

        public void RemoveAppointment(int id);

        public Task AddAppointment(AppointmentView appointment);

        public List<Reason>? GetReasons();
    }
}
