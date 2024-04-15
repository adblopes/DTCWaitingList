using DTCWaitingList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DTCWaitingList
{
    public class AppointmentsDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Appointment> AppointmentsHistory { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonVariant> ReasonVariants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\SQLEXPRESS;Database=DTCWaitingList;Trusted_Connection=True"
            );
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
            
            AppointmentsHistory.Add(appointment);
            Appointments.Remove(appointment);

            SaveChanges();
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
            return [.. Reasons];
        }

        public List<ReasonVariant> GetReasonVariants()
        {
            return [.. ReasonVariants];
        }

        public ReasonVariant GetReasonVariantById(int variantId)
        {
            return ReasonVariants.FirstOrDefault(e => e.Id == variantId)!;
        }

        public void AddReasonVariantt(ReasonVariant variant)
        {
            ReasonVariants.Add(variant);
            SaveChanges();
        }

        public void RemoveReasonVariant(int variantId)
        {
            var variant = GetReasonVariantById(variantId);

            ReasonVariants.Add(variant);
            SaveChanges();
        }


        // Generic method to search for appointments based on any column or combination of columns
        public List<Appointment> SearchAppointments<T>(Expression<Func<Appointment, bool>> predicate) where T : class, new()
        {
            var list = GetAppointments();

            return list.Where(predicate.Compile()).ToList();
        }
    }
}
