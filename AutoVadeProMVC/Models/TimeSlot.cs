using System.ComponentModel.DataAnnotations;

namespace AutoVadeProMVC.Models
{
    public class TimeSlot
    {
        [Key]
        public int Id { get; set; }
        public DateTime SlotBegining { get; set; }
        public DateTime SlotEnding { get; set; }
    }
}
