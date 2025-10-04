using System.ComponentModel.DataAnnotations;

namespace TicketHub.Models
{
    public class Show
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int LocationId { get; set; }
        public Location? Location { get; set; }

        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}