using AutoMapper;
using DTCWaitingList.Interface;
using DTCWaitingList.Models;
using DTCWaitingList.Views;
using Microsoft.IdentityModel.Tokens;

namespace DTCWaitingList.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly WaitingListDb _dbContext;
        private readonly IMapper _mapper;

        public DataAccessService(WaitingListDb dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddAppointment(AppointmentView appointmentView)
        {

            var appointment = new Appointment
            {
                FullName = appointmentView.FullName,
                Email = appointmentView.Email,
                Phone = appointmentView.Phone,
                FullReason = appointmentView.FullReason,
                IsClient = appointmentView.IsClient,
                CreatedDate = appointmentView.CreatedDate ?? DateTime.Now,
            };

            appointment.ReasonId = ProcessReason(appointment.FullReason);

            var appointmentId = await _dbContext.AddAppointmentAsync(appointment);

            await AddAvailability(appointmentView, appointmentId);
        }

        public List<AppointmentView> GetAppointments(string[]? args)
        {
            //parse search parameters
            var appointments = _dbContext.GetAppointments();
            var result = new List<AppointmentView>();

            foreach (Appointment appointment in appointments)
            {
                result.Add(_mapper.Map<AppointmentView>(appointment));
            }

            return result;
        }

        public void RemoveAppointment(int id)
        {
            if (_dbContext.GetAppointmentById(id) != null)
            {
                _dbContext.RemoveAppointment(id);
            }
        }

        public List<Reason> GetReasons()
        {
            return _dbContext.GetReasons();
        }

        private int ProcessReason(string? fullReason)
        {
            var reasons = GetReasons();
            int result = reasons.First().Id;

            if (!fullReason.IsNullOrEmpty())
            {
                var data = reasons.FirstOrDefault(r => fullReason!.Contains(r.ReasonName!, StringComparison.CurrentCultureIgnoreCase));

                if (data != null)
                {
                    result = data.Id;
                }
            
            }

            return result;
        }

        private async Task AddAvailability(AppointmentView appointmentView, int appointmentId)
        {
            if (appointmentView.AvailableDays == null)
            {
                appointmentView.AvailableDays = ["Any Day"];
            }
            if (appointmentView.AvailableTimes == null)
            {
                appointmentView.AvailableTimes = ["Any Time"];

            }

            var days = _dbContext.GetWeekdays();
            var times = _dbContext.GetTimes();

            foreach (string day in appointmentView.AvailableDays!)
            {
                await _dbContext.AddPatientDay(appointmentId, day); 
            }

            foreach (string time in appointmentView.AvailableTimes!)
            {
                await _dbContext.AddPatientTime(appointmentId, time); 
            }
        }
    }
}
