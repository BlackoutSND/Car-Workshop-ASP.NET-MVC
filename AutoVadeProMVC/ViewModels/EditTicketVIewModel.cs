using AutoVadeProMVC.Data.Enums;
using AutoVadeProMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutoVadeProMVC.ViewModels
{
    public class EditTicketVIewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageURL { get; set; }
        public string? DeducedProblem { get; set; }
        public double? PaidPrice { get; set; }
        public int? UserId { get; set; }
        public TicketStatus Status { get; set; }
    }
}
