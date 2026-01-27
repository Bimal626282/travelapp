using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Controllers
{
    [Authorize(Roles = "Agency")]
    [Route("agency")]
    public class AdminController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        [Route("")]
        [Route("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var allBookings = await _context.Bookings.ToListAsync();

            ViewBag.TotalBookings = allBookings.Count;
            ViewBag.PendingBookings = allBookings.Count(b => !b.IsConfirmed);
            ViewBag.ConfirmedBookings = allBookings.Count(b => b.IsConfirmed);
            ViewBag.TotalTravelers = allBookings.Sum(b => b.NumberOfTravelers);

            // Fetch all tours so the Agency can manage both active and hidden ones
            ViewBag.AllTours = await _context.TourPackages.ToListAsync();

            ViewBag.RecentFeedback = await _context.Feedbacks
                .Include(f => f.TourPackage)
                .OrderByDescending(f => f.Id)
                .Take(5)
                .ToListAsync();

            return View(allBookings);
        }

        // --- NEW: Create Tour Package ---
        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourPackage tour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }
            return View(tour);
        }

        // --- NEW: Edit Tour Package ---
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var tour = await _context.TourPackages.FindAsync(id);
            if (tour == null) return NotFound();
            return View(tour);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourPackage tour)
        {
            if (id != tour.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }
            return View(tour);
        }

        // --- NEW: Toggle Status (Soft Delete/Restore) ---
        [HttpPost("toggle-status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var tour = await _context.TourPackages.FindAsync(id);
            if (tour != null)
            {
                tour.IsActive = !tour.IsActive; // Flips between Active and Deleted
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [Route("reports")]
        public async Task<IActionResult> BookingReport()
        {
            var reportData = await _context.Bookings
                .GroupBy(b => b.DestinationName)
                .Select(g => new BookingReportViewModel
                {
                    Destination = g.Key,
                    TotalBookings = g.Count(),
                    TotalTravelers = g.Sum(b => b.NumberOfTravelers),
                    TotalRevenue = g.Count() * 500
                }).ToListAsync();

            return View(reportData);
        }

        [HttpPost("confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost("delete-feedback")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [Route("roles")]
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(roles);
        }
    }
}