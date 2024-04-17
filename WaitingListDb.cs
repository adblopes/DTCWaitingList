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

        public WaitingListDb(DbContextOptions<WaitingListDb> options)
            : base(options)
        {
        }

        public void AddAppointment(Appointment appointment)
        {
            // decide how to validate duplicates
            Appointments.Add(appointment);
            SaveChanges();
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

        public List<Appointment>? GetAppointments()
        {
            return [.. Appointments];
        }

        public List<Reason> GetReasons()
        {
            return [..Reasons];
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
