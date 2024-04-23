using DTCWaitingList.Models;

namespace DTCWaitingList.Interface
{
    public interface IDataAccessService
    {
        public List<Patient> GetPatients();

        public Task RemovePatientAsync(int id);

        public Task AddPatientAsync(Patient patient);

        public Task<List<Reason>>? GetReasonsAsync();
    }
}
