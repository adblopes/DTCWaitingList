using System.ComponentModel.DataAnnotations.Schema;

namespace DTCWaitingList.Models
{
    [Table("Times")]

    public class Time
    {
        public int Id { get; set; }

        public string? TimeOfDay { get; set; }
    }
}
