using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.ViewModels
{
    public class CreateTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Car Car { get; set; }
        public IFormFile? Image { get; set; }
        public int UserId { get; set; }
    }
}
