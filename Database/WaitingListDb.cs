using DTCWaitingList.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DTCWaitingList.Database
{
    public class WaitingListDb : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientHistory> PatientsHistory { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonVariant> ReasonVariants { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<PatientDay> PatientDays { get; set; }
        public DbSet<PatientTime> PatientTimes { get; set; }

        public WaitingListDb(DbContextOptions<WaitingListDb> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientDay>().HasKey(sc => new { sc.PatientId, sc.DayId });

            modelBuilder.Entity<PatientDay>()
                .HasOne(pd => pd.Patient)
                .WithMany(p => p.PatientDays)
                .HasForeignKey(pd => pd.PatientId);

            modelBuilder.Entity<PatientDay>()
                .HasOne(pd => pd.Day)
                .WithMany(p => p.PatientDays)
                .HasForeignKey(pd => pd.DayId);

            modelBuilder.Entity<PatientTime>().HasKey(sc => new { sc.PatientId, sc.TimeId });

            modelBuilder.Entity<PatientTime>()
                .HasOne(pd => pd.Patient)
                .WithMany(p => p.PatientTimes)
                .HasForeignKey(pd => pd.PatientId);

            modelBuilder.Entity<PatientTime>()
                .HasOne(pd => pd.Time)
                .WithMany(p => p.PatientTimes)
                .HasForeignKey(pd => pd.TimeId);

            modelBuilder.Entity<Reason>()
                .HasMany(pd => pd.Patients)
                .WithOne(p => p.Reason)
                .HasForeignKey(pd => pd.ReasonId);

            modelBuilder.Entity<Reason>()
                .HasMany(r => r.ReasonVariants)
                .WithOne(r => r.Reason)
                .HasForeignKey(r => r.ReasonId);

            modelBuilder.Entity<ReasonVariant>().HasKey(sc => new { sc.VariantId });

            modelBuilder.Entity<PatientHistory>()
                .HasKey(sc => new { sc.PatientId });
        }

        public List<Patient> GetPatients()
        {
            //retrieve day and time names to fill in the view later, possible async issue
            var patients = Patients.Include(p => p.PatientDays).Include(p => p.PatientTimes).Include(p => p.Reason).ToList();
            
            var days = Days;
            var times = Times;

            foreach (Patient patient in patients)
            {
                foreach (PatientDay pd in patient.PatientDays)
                {
                    pd.Day = days.First(d => d.DayId == pd.DayId);
                }

                foreach (PatientTime pt in patient.PatientTimes)
                {
                    pt.Time = times.First(t => t.TimeId == pt.TimeId);
                }
            }

            return patients;
        }

        public List<Day> GetDays()
        {
            return [.. Days];
        }

        public List<Time> GetTimes()
        {
            return [.. Times];
        }

        public List<Reason> GetReasons()
        {
            return [.. Reasons];
        }

        public List<ReasonVariant> GetReasonVariants()
        {
            return [.. ReasonVariants];
        }

        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            var patient = await Patients.FirstOrDefaultAsync(e => e.PatientId == patientId);

            return patient;
        }

        // Generic method to search for patients based on any column or combination of columns
        public List<Patient>? SearchPatients<T>(Expression<Func<Patient, bool>> predicate) where T : class, new()
        {
            //var list = GetPatientsAsync();

            //if (list != null)
            //{
            //    return list.Where(predicate.Compile()).ToList();
            //}

            return null;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await Patients.AddAsync(patient);
            await SaveChangesAsync();
        }

        public async Task RemovePatientAsync(Patient patient, PatientHistory patientHistory)
        {
            Patients.Remove(patient);
            await PatientsHistory.AddAsync(patientHistory);
            await SaveChangesAsync();
        }
    }
}
