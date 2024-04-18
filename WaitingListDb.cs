using DTCWaitingList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DTCWaitingList
{
    public class WaitingListDb : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Appointment> AppointmentsHistory { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<PatientDay> PatientDays { get; set; }
        public DbSet<PatientTime> PatientTimes { get; set; }

        public WaitingListDb(DbContextOptions<WaitingListDb> options)
            : base(options)
        {
        }

        public async Task<int> AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                await Appointments.AddAsync(appointment);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (int)appointment.Id!;
        }

        public void RemoveAppointment(int appointmentId)
        {
            var appointment = GetAppointmentById(appointmentId);
            
            if (appointment != null)
            {
                Appointments.Remove(appointment);
                AppointmentsHistory.Add(appointment);
                SaveChanges();
            }
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            return Appointments.FirstOrDefault(e => e.Id == appointmentId)!;
        }

        public List<Appointment> GetAppointments()
        {
            return [.. Appointments];
        }

        public List<Reason> GetReasons()
        {
            return [..Reasons];
        }

        public List<Weekday> GetWeekdays()
        {
            return [..Weekdays];
        }

        public List<Time> GetTimes()
        {
            return [..Times];
        }

        public async Task AddPatientDay(int appointmentId, string day)
        {
            PatientDay pd = new()
            {
                PatientId = appointmentId,
                DayId = GetWeekdays().Find(d => d.NameOfDay == day)!.Id,
            };

            await PatientDays.AddAsync(pd);
            await SaveChangesAsync();
        }
        
        public async Task AddPatientTime(int appointmentId, string time)
        {
            PatientDay pt = new()
            {
                PatientId = appointmentId,
                DayId = GetTimes().Find(d => d.TimeOfDay == time)!.Id,
            };

            await PatientDays.AddAsync(pt);
            await SaveChangesAsync();
        }


        // Generic method to search for appointments based on any column or combination of columns
        public List<Appointment>? SearchAppointments<T>(Expression<Func<Appointment, bool>> predicate) where T : class, new()
        {
            var list = GetAppointments();

            //if (list != null)
            //{
            //    return list.Where(predicate.Compile()).ToList();
            //}

            return null;
        }
    }
}
