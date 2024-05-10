using AutoVadeProMVC.Models;
using AutoVadeProMVC.Data;
using AutoVadeProMVC.Data.Enums;

namespace AutoVadeProMVC.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket> GetTicketByIdAsync(int ticketId);
        Task<Ticket> GetTicketByIdAsyncNoTracking(int ticketId);
        Task<TimeSlot> GetTimeSlotByIdAsync(int id);
        Task<CarPart> GetCarPartByIdAsync(int id);
        Task<int> GetCountAsync();
        Task<int> GetCountWithStatus(TicketStatus status);
        Task<bool> isUserExists(int id);
        bool RemoveCarPart(CarPart carPart);
        bool RemoveTimeSlot(TimeSlot timeSlot);
        bool Add(Ticket ticket);
        bool Update(Ticket ticket);
        bool Delete(Ticket ticket);
        bool Save();
    }
}
