using DTCWaitingList.Models;

namespace DTCWaitingList.Interface
{
    public interface IDataAccessService
    {
        public Task<List<AppointmentView>> GetAppointmentsAsync(string[]? args);

        public Task RemoveAppointmentAsync(int id);

        public Task AddAppointmentAsync(AppointmentView appointment);

        public Task<List<Reason>>? GetReasonsAsync();
    }
}
