using DTCWaitingList.Database.Models;
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

        public void RemovePatient(int id);

        public void AddPatient(PatientView patientView);
    }
}
