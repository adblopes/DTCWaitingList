using DTCWaitingList.Models;

namespace DTCWaitingList.Interface
{
    public interface IDataAccessService
    {
        public List<Appointment> GetAppointments(string[] args);

        public void RemoveAppointment(int id);

        public void AddAppointment(Appointment appointment);

        public List<Reason> GetReasons();

        public List<ReasonVariant> GetReasonVariants();
    }
}
