namespace DTCWaitingList.Models
{
    public class PatientView
    {
        public int? PatientId { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }
        
        public string? Reason { get; set; }

        public DateTime? CreatedDate { get; set; }

        public IList<string>? PatientDays { get; set; }

        public IList<string>? PatientTimes { get; set; }
    }
}
