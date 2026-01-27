using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TravelApp.Data;
using TravelApp.Models;

namespace TravelApp.Controllers
{
    [Authorize] // Prevents anonymous access
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Feedback/Create?tourId=5
        [HttpGet]
        public IActionResult Create(int tourId)
        {
            var feedback = new Feedback { TourPackageId = tourId };
            return View(feedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.TouristName = User.Identity?.Name ?? "Anonymous";
                _context.Add(feedback);
                await _context.SaveChangesAsync();

                // Redirects back to the Booking Details page
                return RedirectToAction("Details", "Booking", new { id = feedback.TourPackageId });
            }
            return View(feedback);
        }
    }
}