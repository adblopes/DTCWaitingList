using DTCWaitingList.Interface;
using DTCWaitingList.Models;

namespace DTCWaitingList.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly WaitingListDb _dbContext;

        public DataAccessService(WaitingListDb dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddAppointment(Appointment appointment)
        {
            _dbContext.AddAppointment(appointment);
        }

        public List<Appointment>? GetAppointments(string[]? args)
        {
            //parse search parameters
            return _dbContext.GetAppointments();
        }

        public void RemoveAppointment(int id)
        {
            if (_dbContext.GetAppointmentById(id) != null)
            {
                _dbContext.RemoveAppointment(id);
            }
        }

        public List<Reason>? GetReasons()
        {
            return _dbContext.GetReasons();
        }
    }
}
