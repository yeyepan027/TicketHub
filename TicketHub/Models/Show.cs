using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketHub.Models
{
    public class Show
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Foreign key and navigation property for Category
        public int CategoryId { get; set; }    // nullable if optional
        public Category? Category { get; set; }

        // Foreign key and navigation property for Location
        public int LocationId { get; set; }
        public Location? Location { get; set; }

        // Foreign key and navigation property for Owner
        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
