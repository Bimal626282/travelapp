using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Data;
using TravelApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace TravelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- MAIN LANDING PAGE ---
        public async Task<IActionResult> Index()
        {
            // 1. Fetch available tours for everyone to see
            var tours = await _context.TourPackages.ToListAsync();

            // 2. Initialize an empty list for bookings
            List<Booking> userBookings = new List<Booking>();

            // 3. Logic: Only fetch bookings if the user is a logged-in Tourist
            // This prevents Agency users and Guests from seeing the "Upcoming Trips" data
            if (User.Identity.IsAuthenticated && !User.IsInRole("Agency"))
            {
                // Use the authenticated user's Id (not name) because Booking stores UserId
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    userBookings = await _context.Bookings
                        .Where(b => b.UserId == userId)
                        .ToListAsync();
                }
            }

            // 4. Map the gated data to the HomeViewModel
            var viewModel = new HomeViewModel
            {
                TourPackages = tours,
                MyBookings = userBookings
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}