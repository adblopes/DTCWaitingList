using System.ComponentModel.DataAnnotations.Schema;

namespace DTCWaitingList.Models
{

    public class AppointmentView
    {
        public int AppointmentId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ReasonId { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }

        public string? DayOfWeek { get; set; }

        public string? TimeOfDay { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
