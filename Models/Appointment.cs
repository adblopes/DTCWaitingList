namespace DTCWaitingList.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public List<string>? AvailableDays { get; set; }

        public List<string>? AvailableTimes { get; set; }

        public DateTime NextAppointment { get; set; }

        public List<Reason>? Reasons { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
