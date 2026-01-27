using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelApp.Data;
using TravelApp.Models;
using System.Security.Claims;

namespace TravelApp.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Details Action ---
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Home");

            // Updated to ensure Feedbacks are ordered by newest first
            var tour = await _context.TourPackages
                .Include(t => t.Feedbacks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tour == null) return NotFound();

            return View(tour);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create(string destination, decimal? price, int? duration)
        {
            ViewBag.Destination = destination;
            ViewBag.Price = price;
            ViewBag.Duration = duration;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DestinationName,BookingDate,NumberOfTravelers")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Assign current logged-in user's Id to the booking
                booking.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
                booking.IsConfirmed = false;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyBookings));
        }
    }
}