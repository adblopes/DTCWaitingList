using DTCWaitingList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DTCWaitingList
{
    public class WaitingListDb : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentView> AppointmentViews { get; set; }
        public DbSet<Appointment> AppointmentsHistory { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonVariant> ReasonVariants { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<PatientDay> PatientDays { get; set; }
        public DbSet<PatientTime> PatientTimes { get; set; }

        public WaitingListDb(DbContextOptions<WaitingListDb> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentView>(eb =>
            {
                eb.ToView("AppointmentViews");
                eb.HasKey(a => a.AppointmentId);
            });
        }

        public async Task<List<AppointmentView>> GetAppointmentsAsync()
        {
            var a = await AppointmentViews.ToListAsync();

            return a;

        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await Appointments.FirstOrDefaultAsync(e => e.Id == appointmentId);

            return appointment;
        }

        // Generic method to search for appointments based on any column or combination of columns
        public List<AppointmentView>? SearchAppointments<T>(Expression<Func<Appointment, bool>> predicate) where T : class, new()
        {
            var list = GetAppointmentsAsync();

            //if (list != null)
            //{
            //    return list.Where(predicate.Compile()).ToList();
            //}

            return null;
        }

        public async Task<int> AddAppointmentAsync(Appointment appointment)
        {
            await Appointments.AddAsync(appointment);
            await SaveChangesAsync();

            return (int)appointment.Id!;
        }

        public async Task RemoveAppointmentAsync(int appointmentId)
        {
            var appointment = await GetAppointmentByIdAsync(appointmentId);
            
            if (appointment != null)
            {
                Appointments.Remove(appointment);
                await AppointmentsHistory.AddAsync(appointment);

                PatientDays.RemoveRange(PatientDays.Where(x => x.PatientId == appointmentId));
                PatientTimes.RemoveRange(PatientTimes.Where(x => x.PatientId == appointmentId));

                await SaveChangesAsync();
            }
        }

        public async Task<List<Reason>> GetReasonsAsync()
        {
            return await Reasons.ToListAsync();
        }
        
        public async Task<List<ReasonVariant>> GetReasonVariantsAsync()
        {
            return await ReasonVariants.ToListAsync();
        }

        public async Task<List<Weekday>> GetWeekdaysAsync()
        {
            return await Weekdays.ToListAsync();
        }

        public async Task<List<Time>> GetTimesAsync()
        {
            return await Times.ToListAsync();
        }

        public async Task AddPatientDayAsync(int appointmentId, string day)
        {
            var days = await GetWeekdaysAsync();

            PatientDay pd = new()
            {
                PatientId = appointmentId,
                DayId = days.Find(d => d.NameOfDay == day)!.Id,
            };

            await PatientDays.AddAsync(pd);
            await SaveChangesAsync();
        }

        public async Task AddPatientTimeAsync(int appointmentId, string time)
        {
            var times = await GetTimesAsync();

            PatientTime pt = new()
            {
                PatientId = appointmentId,
                TimeId = times.Find(d => d.TimeOfDay == time)!.Id,
            };

            await PatientTimes.AddAsync(pt);
            await SaveChangesAsync();
        }
    }
}
