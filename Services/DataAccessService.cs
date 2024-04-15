using DTCWaitingList.Interface;
using DTCWaitingList.Models;

namespace DTCWaitingList.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly AppointmentsDbContext _dbContext;

        public DataAccessService(AppointmentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddAppointment(Appointment appointment)
        {
            _dbContext.AddAppointment(appointment);
        }

        public List<Appointment> GetAppointments(string[] args)
        {
            //parse search parameters
            return _dbContext.GetAppointments();
        }

        public List<Reason> GetReasons()
        {
            return _dbContext.GetReasons();
        }

        public List<ReasonVariant> GetReasonVariants()
        {
            return _dbContext.GetReasonVariants();
        }

        public void RemoveAppointment(int id)
        {
            if (_dbContext.GetAppointmentById(id) != null)
            {
                _dbContext.RemoveAppointment(id);
            }
        }
    }
}
