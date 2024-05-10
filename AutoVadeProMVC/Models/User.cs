using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace AutoVadeProMVC.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname {  get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public double? Wage { get; set; }
        public string? Image { get; set; }
        public bool IsAdmin { get; set; }
        [ForeignKey("Ticket")]
        public List<int>? TicketIds { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
