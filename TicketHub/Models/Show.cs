
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketHub.Models
{
   
    public class Show
    {
        // Primary key for the Show entity
        public int Id { get; set; }

        // Title of the show (required field)
        [Required]
        public string Title { get; set; } = string.Empty;

        // Optional description of the show
        public string? Description { get; set; }

        // Foreign key to the Category entity
        public int CategoryId { get; set; }

        // Navigation property to access the related Category
        public Category? Category { get; set; }

        // Foreign key to the Location entity
        public int LocationId { get; set; }

        // Navigation property to access the related Location
        public Location? Location { get; set; }

        // Foreign key to the Owner entity
        public int OwnerId { get; set; }

        // Navigation property to access the related Owner
        public Owner? Owner { get; set; }

        // Date of the show (formatted as a date)
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Time of the show (formatted as a time)
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        // Timestamp when the show record was created
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string? ImageFilename { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Event Image")]
        public IFormFile? ImageFile { get; set; }




        // ✅ Add this for purchases
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();


    }
}