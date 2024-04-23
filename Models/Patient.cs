namespace DTCWaitingList.Models
{
    public class Patient
    {
        public int? PatientId { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }
        
        public int ReasonId { get; set; }

        public Reason? Reason { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<PatientDay>? PatientDays { get; set; }

        public IList<PatientTime>? PatientTimes { get; set; }
    }
}
