using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelApp.Models;

namespace TravelApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This line creates the physical 'Bookings' table in SQL
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TourPackage> TourPackages { get; set; }
    }
}