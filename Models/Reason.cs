namespace DTCWaitingList.Models
{
    public class Reason
    {
        public int ReasonId { get; set; }

        public string? ReasonName { get; set; }

        public IList<Patient>? Patients { get; set; }

        public IList<ReasonVariant>? ReasonVariants { get; set; }
    }
}
