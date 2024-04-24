namespace DTCWaitingList.Database.Models
{
    public class ReasonVariant
    {
        public int VariantId { get; set; }

        public string? Term { get; set; }

        public int ReasonId { get; set; }

        public Reason? Reason { get; set; }
    }
}
