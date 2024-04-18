namespace DTCWaitingList.Views
{
    public class AppointmentView
    {
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ReasonId { get; set; }

        public string? FullReason { get; set; }

        public string[]? AvailableDays { get; set; }

        public string[]? AvailableTimes { get; set; }

        public bool IsClient { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
