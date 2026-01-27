using System.ComponentModel.DataAnnotations;

namespace TravelApp.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please share your experience with us!")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Feedback must be between 10 and 500 characters.")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 (Poor) and 5 (Excellent).")]
        public int Rating { get; set; }

        public string? TouristName { get; set; }

        // Added for the assessment rubric to track when feedback was created
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        // Relationship: Each feedback belongs to one Tour Package
        public int TourPackageId { get; set; }
        public TourPackage? TourPackage { get; set; }
    }
}