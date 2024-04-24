namespace DTCWaitingList.Database.Models
{
    public class Day
    {
        public int DayId { get; set; }

        public string? NameOfDay { get; set; }

        public IList<PatientDay>? PatientDays { get; set; }
    }
}
