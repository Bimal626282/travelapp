using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelApp.Models
{
    public class TourPackage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tour title is required")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Destination name is required for booking logic")]
        public string Destination { get; set; }

        [Required(ErrorMessage = "Specific location is required")]
        public string Location { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public string AgencyName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000")]
        public decimal Price { get; set; }

        [Required]
        public int Duration { get; set; }

        [Display(Name = "Duration (Days)")]
        public int DurationDays { get; set; }

        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Available To")]
        public DateTime AvailableTo { get; set; } = DateTime.Now.AddDays(7);

        // --- SOFT DELETE PROPERTY ---
        // True = Visible to Tourists | False = Hidden (Deleted)
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}