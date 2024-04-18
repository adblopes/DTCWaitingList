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

        public async Task AddAppointmentAsync(AppointmentView appointmentView)
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

            appointment.ReasonId = await ProcessReasonAsync(appointment.FullReason);

            var appointmentId = await _dbContext.AddAppointmentAsync(appointment);

            await AddAvailabilityAsync(appointmentView, appointmentId);
        }

        public async Task<List<AppointmentView>> GetAppointmentsAsync(string[]? args)
        {
            //parse search parameters
            var appointments = await _dbContext.GetAppointmentsAsync();
            var result = new List<AppointmentView>();

            foreach (Appointment appointment in appointments)
            {
                result.Add(_mapper.Map<AppointmentView>(appointment));
            }

            return result;
        }

        public async Task RemoveAppointmentAsync(int id)
        {
            var appointment = await _dbContext.GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                await _dbContext.RemoveAppointmentAsync(id);
            }
        }

        public async Task<List<Reason>> GetReasonsAsync()
        {
            return await _dbContext.GetReasonsAsync();
        }
        
        public async Task<List<ReasonVariant>> GetReasonVariantsAsync()
        {
            return await _dbContext.GetReasonVariantsAsync();
        }

        private async Task<int> ProcessReasonAsync(string? fullReason)
        {
            var reasons = await GetReasonsAsync();
            var variants = await GetReasonVariantsAsync();
            int result = reasons.First().Id;

            if (!fullReason.IsNullOrEmpty())
            {
                var data = variants.FirstOrDefault(r => fullReason!.Contains(r.Term!, StringComparison.CurrentCultureIgnoreCase));

                if (data != null)
                {        
                    result = reasons.Find(r => r.Id == data.ReasonId)!.Id;
                }
            
            }

            return result;
        }

        private async Task AddAvailabilityAsync(AppointmentView appointmentView, int appointmentId)
        {
            if (appointmentView.AvailableDays == null)
            {
                appointmentView.AvailableDays = ["Any Day"];
            }
            if (appointmentView.AvailableTimes == null)
            {
                appointmentView.AvailableTimes = ["Any Time"];

            }

            foreach (string day in appointmentView.AvailableDays!)
            {
                await _dbContext.AddPatientDayAsync(appointmentId, day); 
            }

            foreach (string time in appointmentView.AvailableTimes!)
            {
                await _dbContext.AddPatientTimeAsync(appointmentId, time); 
            }
        }
    }
}
