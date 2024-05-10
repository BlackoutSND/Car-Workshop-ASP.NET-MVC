namespace AutoVadeProMVC.ViewModels
{
    public class CreateUserViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Wage { get; set; }
        public bool IsAdmin { get; set; }
        public IFormFile? Image { get; set; }
    }
}
