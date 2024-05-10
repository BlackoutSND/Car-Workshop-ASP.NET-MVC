using Microsoft.EntityFrameworkCore;
using AutoVadeProMVC.Data;
using AutoVadeProMVC.Interfaces;
using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.Services
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(User user)
        {
            _context.Users.Add(user);
            return Save();
        }
        public bool Delete(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                                    .Include(y => y.Tickets)
                                    .ThenInclude(z => z.TimeSlots)
                                    .FirstOrDefaultAsync(i=>i.Id==id);
        }
        public async Task<User> GetUserByIdNoTracking(int id)
        {
            return await _context.Users.AsNoTracking()
                                    .Include(y => y.Tickets)
                                    .ThenInclude(z => z.TimeSlots)
                                    .FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }
        public async Task<User> GetUserByLoginNoTracking(string login)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Login == login);
        }
        public async Task<User> GetUserByLoginPassword(string login, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(i=>i.Login==login && i.Password==password);
        }
    }
}
