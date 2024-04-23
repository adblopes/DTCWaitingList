namespace DTCWaitingList.Models
{
    public class Time
    {
        public int TimeId { get; set; }

        public string? TimeOfDay { get; set; }
        
        public IList<PatientTime>? PatientTimes { get; set; }
    }
}
