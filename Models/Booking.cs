using System.ComponentModel.DataAnnotations;

namespace TravelApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string DestinationName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Travel Date")]
        public DateTime BookingDate { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "You can book for 1 to 10 people.")]
        [Display(Name = "Number of Travelers")]
        public int NumberOfTravelers { get; set; }

        public bool IsConfirmed { get; set; }

        // Link to the Identity User (The person who made the booking)
        public string? UserId { get; set; }

    }
}