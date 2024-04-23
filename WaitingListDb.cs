using DTCWaitingList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DTCWaitingList
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
                .HasOne<Patient>(pd => pd.Patient)
                .WithMany(p => p.PatientDays)
                .HasForeignKey(pd => pd.PatientId);

            modelBuilder.Entity<PatientDay>()
                .HasOne<Day>(pd => pd.Day)
                .WithMany(p => p.PatientDays)
                .HasForeignKey(pd => pd.DayId);

            modelBuilder.Entity<PatientTime>().HasKey(sc => new { sc.PatientId, sc.TimeId });

            modelBuilder.Entity<PatientTime>()
                .HasOne<Patient>(pd => pd.Patient)
                .WithMany(p => p.PatientTimes)
                .HasForeignKey(pd => pd.PatientId);

            modelBuilder.Entity<PatientTime>()
                .HasOne<Time>(pd => pd.Time)
                .WithMany(p => p.PatientTimes)
                .HasForeignKey(pd => pd.TimeId);

            modelBuilder.Entity<Reason>()
                .HasMany<Patient>(pd => pd.Patients)
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

        //https://learn.microsoft.com/en-us/ef/core/miscellaneous/async
        //known issues with async queries
        public List<Patient> GetPatients()
        {
            return Patients.Include(p => p.PatientDays).Include(p => p.PatientTimes).Include(p => p.Reason).ToList();
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

        public async Task<int> AddPatientAsync(Patient patient)
        {
            await Patients.AddAsync(patient);
            await SaveChangesAsync();

            return (int)patient.PatientId!;
        }

        public async Task RemovePatientAsync(int patientId)
        {
            var patient = await GetPatientByIdAsync(patientId);
            
            if (patient != null)
            {
                Patients.Remove(patient);
                //await PatientsHistory.AddAsync(appointment);

                //PatientDays.RemoveRange(PatientDays.Where(x => x.PatientId == appointmentId));
                //PatientTimes.RemoveRange(PatientTimes.Where(x => x.PatientId == appointmentId));

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

        public async Task<List<Day>> GetWeekdaysAsync()
        {
            return await Days.ToListAsync();
        }

        public async Task<List<Time>> GetTimesAsync()
        {
            return await Times.ToListAsync();
        }
    }
}
