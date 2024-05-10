using Microsoft.EntityFrameworkCore;
using AutoVadeProMVC.Data;
using AutoVadeProMVC.Data.Enums;
using AutoVadeProMVC.Interfaces;
using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.Services
{
    public class TicketRepository:ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Ticket ticket)
        {
            _context.Add(ticket);
            return Save();
        }

        public bool Delete(Ticket ticket)
        {
            _context.Remove(ticket);
            return Save();
        }
        public bool RemoveTimeSlot(TimeSlot timeSlot)
        {
            _context.TimeSlots.Remove(timeSlot);
            return Save();
        }
        public bool RemoveCarPart(CarPart carPart)
        {
            _context.CarParts.Remove(carPart);
            return Save();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Tickets.CountAsync();
        }

        public async Task<int> GetCountWithStatus(TicketStatus status)
        {
            return await _context.Tickets.CountAsync(i=>i.Status == status);
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets
                                        .Include(y => y.CarParts)
                                        .Include(y => y.Car)
                                        .Include(y => y.TimeSlots)
                                        .Include(y => y.User)
                                        .FirstOrDefaultAsync(y => y.Id == ticketId);
        }
        public async Task<Ticket> GetTicketByIdAsyncNoTracking(int ticketId)
        {
            return await _context.Tickets.AsNoTracking()
                                        .Include(y => y.CarParts)
                                        .Include(y => y.Car)
                                        .Include(y => y.TimeSlots)
                                        .Include(y => y.User)
                                        .FirstOrDefaultAsync(y => y.Id == ticketId);
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Ticket ticket)
        {
            _context.Update(ticket);
            return Save();
        }

        public async Task<bool> isUserExists(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Id == id) != null;
        }
        public async Task<TimeSlot> GetTimeSlotByIdAsync(int id)
        {
            return await _context.TimeSlots.FirstOrDefaultAsync(y => y.Id == id);
        }
        public async Task<CarPart> GetCarPartByIdAsync(int id)
        {
            return await _context.CarParts.FirstOrDefaultAsync(y => y.Id == id);
        }
    }
}
