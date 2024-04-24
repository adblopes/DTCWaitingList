namespace DTCWaitingList.Models
{
    public class PatientDay
    {
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public int DayId { get; set; }

        public Day? Day { get; set; }
    }
}
