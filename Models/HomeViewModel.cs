using System.Collections.Generic;

namespace TravelApp.Models
{
    public class HomeViewModel
    {
        // This will hold the list of available tours (The cards)
        public IEnumerable<TourPackage> TourPackages { get; set; }

        // This will hold the logged-in user's specific bookings
        public IEnumerable<Booking> MyBookings { get; set; }
    }
}