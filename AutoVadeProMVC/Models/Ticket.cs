 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoVadeProMVC.Data.Enums;

namespace AutoVadeProMVC.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        public double? ApproximatePrice { get; set; }
        public string? DeducedProblem { get; set; }
        public double? PaidPrice { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("CarPart")]
        public List<int>? CarPartId { get; set; }
        public List<CarPart>? CarParts { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
        [ForeignKey("TimeSlot")]
        public List<int>? TimeSlotIds { get; set; }
        public List<TimeSlot>? TimeSlots { get; set; }
        public TicketStatus Status { get; set; }
    }
}
