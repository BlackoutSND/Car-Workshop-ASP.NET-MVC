using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoVadeProMVC.Data;
using AutoVadeProMVC.Interfaces;
using AutoVadeProMVC.Models;
using AutoVadeProMVC.Services;
using AutoVadeProMVC.ViewModels;
using System.Net.Sockets;

namespace AutoVadeProMVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketService;
        private readonly IPhotoService _photoService;

        public TicketController(ITicketRepository ticketService, IPhotoService photoService)
        {
            _ticketService = ticketService;
            _photoService = photoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            Ticket? ticket=await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }
        public async Task<IActionResult> List()
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            var Tickets = await _ticketService.GetTickets();
            return View(Tickets);
        }
        public async Task<IActionResult> Create()
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketViewModel ticketVM)
        {
            bool check = await _ticketService.isUserExists((int)ticketVM.UserId);
            if (!check)
            {
                ModelState.AddModelError("UserId", "Employee with the provided Id does not exist!");
            }
            else if (ModelState.IsValid)
            {
               
                var ticket = new Ticket
                {
                    Title = ticketVM.Title,
                    Description = ticketVM.Description,
                    UserId = ticketVM.UserId,
                    Car = new Car
                    {
                        Brand = ticketVM.Car.Brand,
                        Model = ticketVM.Car.Model,
                        RegistrationId = ticketVM.Car.RegistrationId
                    },
                };
                if (ticketVM.Image != null)
                {
                    var result = await _photoService.AddPhotoAsync(ticketVM.Image);
                    ticket.Image = result.Url.ToString();
                }
                
                _ticketService.Add(ticket);
                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("Image", "Photo upload has been failed!");
            }
            return View(ticketVM);

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            Ticket? ticketDetails = await _ticketService.GetTicketByIdAsync(id);
            if (ticketDetails == null)
            {
                return RedirectToAction("List");
            }
            return View(ticketDetails);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticketDetails = await _ticketService.GetTicketByIdAsync(id);
            if (ticketDetails == null)
            {
                return NotFound();
            }
            _ticketService.Delete(ticketDetails);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (LoggedInUser.Id != -1)
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                    return NotFound();
                var ticketVM = new EditTicketVIewModel
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    Status = ticket.Status,
                    ImageURL = ticket.Image,
                    DeducedProblem  = ticket.DeducedProblem,
                    PaidPrice   = ticket.PaidPrice,
                    UserId = ticket.UserId,
                };
                return View(ticketVM);
            }
            return RedirectToAction("List");

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTicketVIewModel ticketVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to ticket User!");
                return View("Edit", ticketVM);
            }
            var ticketReq = await _ticketService.GetTicketByIdAsyncNoTracking(id);
            if (ticketReq != null)
            {
                var ticket = new Ticket
                {
                    Id = ticketVM.Id,
                    Title = ticketVM.Title,
                    Description = ticketVM.Description,
                    Status =Data.Enums.TicketStatus.Pending,
                    DeducedProblem = ticketVM.DeducedProblem,
                    PaidPrice = ticketVM.PaidPrice,
                    UserId = ticketVM.UserId,
                    CarId = ticketReq.CarId,


                };
                if (ticketReq.Image != null)
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(ticketReq.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete photo!");
                        return View(ticketReq);
                    }

                }
                if (ticketVM.Image != null)
                {
                    var result = await _photoService.AddPhotoAsync(ticketVM.Image);
                    ticket.Image = result.Url.ToString();
                }
                _ticketService.Update(ticket);
                return RedirectToAction("List");
            }
            else
            {
                return View(ticketVM);
            }

        }
        
        public async Task<IActionResult> AddPart(int Id)
        {
            if(LoggedInUser.Id!=-1)
                return View();
            else return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> AddPart(int Id, AddPartTicketViewModel partVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Could not add part!");
                return View("AddPart", partVM);
            }
            var ticketReq = await _ticketService.GetTicketByIdAsyncNoTracking(Id);
            if (ticketReq != null)
            {
                var ticket = new Ticket();
                ticket = ticketReq;
                ticket.CarParts.Add(new CarPart {
                    Name=partVM.Name,
                    Quantity=partVM.Quantity,
                    Price=partVM.Price
                });
                _ticketService.Update(ticket);
                return RedirectToAction("Detail", new { Id });
            }
            ModelState.AddModelError("", "Could not add part!");
            return View("AddPart", partVM);
        }
        public async Task<IActionResult> TicketIsDone(int id)
        {
            if (LoggedInUser.Id != -1)
            {
                var ticketReq = await _ticketService.GetTicketByIdAsyncNoTracking(id);
                if (ticketReq != null)
                {
                    var ticket = new Ticket();
                    ticket = ticketReq;
                    ticket.Status = Data.Enums.TicketStatus.Done;
                    _ticketService.Update(ticket);
                }

            }
            return RedirectToAction("Detail", new { id });
        }
        
        public async Task<IActionResult> TicketIsClosed(int id)
        {
            if (LoggedInUser.Id != -1)
            {
                var ticketReq = await _ticketService.GetTicketByIdAsyncNoTracking(id);
                if (ticketReq != null)
                {
                    var ticket = new Ticket();
                    ticket = ticketReq;
                    ticket.Status = Data.Enums.TicketStatus.Closed;
                    _ticketService.Update(ticket);
                }

            }
            return RedirectToAction("Detail", new { id });
        }
        public IActionResult AddTimeSlot (int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeSlot(int id,AddTimeSlotTicketViewModel timeSlotVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Could not add time slot!");
                return View("AddTimeSlot", timeSlotVM);
            }
            else if (timeSlotVM.SlotBegining > timeSlotVM.SlotEnding)
            {
                ModelState.AddModelError("SlotBegining", "The begining of the slot is after its end!");
                ModelState.AddModelError("SlotBegining", "The ending of the slot is before its begining!");
                return View("AddTimeSlot", timeSlotVM);
            }
            else if(timeSlotVM.SlotBegining == timeSlotVM.SlotEnding && timeSlotVM.StartingHour == timeSlotVM.EndingHour) { 
                ModelState.AddModelError("StartingHour", "The length of the slot should be at least ONE hour!");
                ModelState.AddModelError("EndingHour", "The length of the slot should be at least ONE hour!");
                return View("AddTimeSlot", timeSlotVM);
            }
            var ticketReq = await _ticketService.GetTicketByIdAsyncNoTracking(id);
            if (ticketReq != null)
            {
                TimeSlot timeSlot = new TimeSlot()
                {
                    SlotBegining= timeSlotVM.SlotBegining.AddHours(timeSlotVM.StartingHour),
                    SlotEnding = timeSlotVM.SlotEnding.AddHours(timeSlotVM.EndingHour)

                };

                var ticket = new Ticket();
                ticket = ticketReq;
                ticket.TimeSlots.Add(timeSlot);
                _ticketService.Update(ticket);
            }
                return RedirectToAction("Detail", new { id });
        }
        public async Task<IActionResult> RemoveTimeSlot(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            var timeslotDetails = await _ticketService.GetTimeSlotByIdAsync(id);
            if (timeslotDetails == null)
            {
                return NotFound();
            }
            _ticketService.RemoveTimeSlot(timeslotDetails);
            return RedirectToAction("List");
        }
        public async Task<IActionResult> RemoveCarPart(int id)
        {
            if(LoggedInUser.Id==-1)
                return RedirectToAction("SignIn", "User");
            var carPartDetails = await _ticketService.GetCarPartByIdAsync(id);
            if (carPartDetails == null)
            {
                return NotFound();
            }
            _ticketService.RemoveCarPart(carPartDetails);
            return RedirectToAction("List");
        }

    }
}
