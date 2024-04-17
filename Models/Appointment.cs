using System.ComponentModel.DataAnnotations.Schema;

namespace DTCWaitingList.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int? ReasonId { get; set; }

        public string? FullReason { get; set; }

        public bool IsClient { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
