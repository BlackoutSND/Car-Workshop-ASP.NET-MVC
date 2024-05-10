using System.ComponentModel.DataAnnotations;

namespace AutoVadeProMVC.Models
{
    public class CarPart
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set;}
        public double Quantity { get; set;}
    }
}
