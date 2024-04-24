using DTCWaitingList.Models;

namespace DTCWaitingList.Interfaces
{
    public interface IDataAccessService
    {
        public List<PatientView> GetPatients();

        public List<Day> GetDays();

        public List<Time> GetTimes();

        public List<Reason> GetReasons();

        public List<ReasonVariant> GetReasonVariants();

        public Task RemovePatientAsync(int id);

        public Task AddPatientAsync(PatientView patientView);
    }
}
