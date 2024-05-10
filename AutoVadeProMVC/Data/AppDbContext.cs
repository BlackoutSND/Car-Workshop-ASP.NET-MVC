using Microsoft.EntityFrameworkCore;
using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users {  get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarPart> CarParts { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

    }
}
