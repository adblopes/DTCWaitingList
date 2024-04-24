namespace DTCWaitingList.Database.Models
{
    public class PatientHistory
    {
        public int? PatientId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }

        public Reason? Reason { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
