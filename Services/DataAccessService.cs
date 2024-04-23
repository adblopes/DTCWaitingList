using DTCWaitingList.Interface;
using DTCWaitingList.Models;
using Microsoft.IdentityModel.Tokens;

namespace DTCWaitingList.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly WaitingListDb _dbContext;

        public DataAccessService(WaitingListDb dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Patient> GetPatients()
        {
            return _dbContext.GetPatients();
        }

        public async Task AddPatientAsync(Patient appointmentView)
        {

            var appointment = new Patient
            {
                FullName = appointmentView.FullName,
                Email = appointmentView.Email,
                Phone = appointmentView.Phone,
                FullReason = appointmentView.FullReason,
                IsClient = appointmentView.IsClient,
                //CreatedDate = appointmentView.CreatedDate ?? DateTime.Now,
            };

            //appointment.ReasonId = await ProcessReasonAsync(appointment.FullReason);

            var appointmentId = await _dbContext.AddPatientAsync(appointment);

            //await AddAvailabilityAsync(appointmentView, appointmentId);
        }

        //public async Task<List<Patient>> GetPatientsAsync(string[]? args)
        //{
        //    //parse search parameters
        //    return await _dbContext.GetPatientsAsync();
        //}

        public async Task RemovePatientAsync(int id)
        {
            var appointment = await _dbContext.GetPatientByIdAsync(id);
            if (appointment != null)
            {
                await _dbContext.RemovePatientAsync(id);
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
            int result = reasons.First().ReasonId;

            if (!fullReason.IsNullOrEmpty())
            {
                var data = variants.FirstOrDefault(r => fullReason!.Contains(r.Term!, StringComparison.CurrentCultureIgnoreCase));

                if (data != null)
                {        
                    result = reasons.Find(r => r.ReasonId == data.ReasonId)!.ReasonId;
                }
            
            }

            return result;
        }

        //private async Task AddAvailabilityAsync(AppointmentView appointmentView, int appointmentId)
        //{
        //    if (appointmentView.DayOfWeek == null)
        //    {
        //        appointmentView.DayOfWeek = ["Any Day"];
        //    }
        //    if (appointmentView.TimeOfDay == null)
        //    {
        //        appointmentView.TimeOfDay = ["Any Time"];

        //    }

        //    foreach (string day in appointmentView.DayOfWeek!)
        //    {
        //        await _dbContext.AddPatientDayAsync(appointmentId, day);
        //    }

        //    foreach (string time in appointmentView.TimeOfDay!)
        //    {
        //        await _dbContext.AddPatientTimeAsync(appointmentId, time);
        //    }
        //}
    }
}
