using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Net;
using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<User>()
                    {
                        new User()
                        {
                            Name = "Test",
                            Surname = "Test",
                            Login = "Test",
                            Password = "Test",
                            Wage = 0.01,
                            IsAdmin = false
                         },
                        new User()
                        {
                            Name = "Admin",
                            Surname = "Admin",
                            Login = "Admin",
                            Password = "Admin",
                            Wage = 10000,
                            IsAdmin = true
                         },
                        new User()
                        {
                            Name = "Emp1",
                            Surname = "1pmE",
                            Login = "Guest",
                            Password = "Guest",
                            Wage = 0.01,
                            IsAdmin = false

                         },
                        new User()
                        {
                            Name = "Figa",
                            Surname = "Giga",
                            Login = "Figa1",
                            Password = "Giga2",
                            Wage = 69420,
                            IsAdmin = true,
                            Tickets = new List<Ticket>()
                            {
                                new Ticket()
                                {
                                    Id = 1,
                                    Title = "Test Ticket 1",
                                    Description = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
                                    ApproximatePrice = 0.001,
                                    UserId = 4,
                                    Car = new Car()
                                    {
                                        Id = 1,
                                        Brand = "Toyota",
                                        Model = "Testa",
                                        RegistrationId = "FillMeup"
                                    },
                                    CarParts = new List<CarPart>()
                                    {
                                        new CarPart()
                                        {
                                            Name = "Engine",
                                            Price = 1000,
                                            Quantity = 15

                                        },
                                        new CarPart()
                                        {
                                            Name = "Gas Pump",
                                            Price = 565665,
                                            Quantity = 1

                                        }
                                    },
                                    Status = Enums.TicketStatus.Pending,
                                    TimeSlots = new List<TimeSlot>()
                                    {
                                        new TimeSlot()
                                        {
                                            SlotBegining = new DateTime(2024, 06, 14, 12, 30, 0),
                                            SlotEnding = new DateTime(2024, 06, 14, 16, 30, 0)
                                        },
                                        new TimeSlot()
                                        {
                                             SlotBegining = new DateTime(2024, 07, 12, 17, 30, 0),
                                            SlotEnding = new DateTime(2024, 07, 12, 18, 30, 0)
                                        },
                                    }


                                }
                            }
                         }
                    });
                    context.SaveChanges();
                }           
            }
        }
    }
}
