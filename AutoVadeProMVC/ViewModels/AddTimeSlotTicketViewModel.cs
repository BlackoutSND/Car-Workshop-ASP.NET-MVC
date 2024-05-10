namespace AutoVadeProMVC.ViewModels
{
    public class AddTimeSlotTicketViewModel
    {
        public int Id { get; set; }
        public DateTime SlotBegining { get; set; }
        public DateTime SlotEnding { get; set; }
        public int StartingHour { get; set; }
        public int EndingHour { get; set;}
    }
}
