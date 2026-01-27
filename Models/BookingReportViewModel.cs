namespace TravelApp.Models
{
    public class BookingReportViewModel
    {
        // Initialize to avoid CS8618: ensure non-nullable property has a value
        public string Destination { get; set; } = string.Empty;
        public int TotalBookings { get; set; }
        public int TotalTravelers { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}