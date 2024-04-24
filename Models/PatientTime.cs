namespace DTCWaitingList.Models
{
    public class PatientTime
    {
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int TimeId { get; set; }

        public Time? Time { get; set; }
    }
}
