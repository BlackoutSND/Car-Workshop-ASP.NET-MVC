using System.ComponentModel.DataAnnotations;

namespace AutoVadeProMVC.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationId { get; set; }

    }
}
