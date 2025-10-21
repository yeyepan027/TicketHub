using System.ComponentModel.DataAnnotations.Schema;
using TicketHub.Models;

public class Location
{
        // Primary key for the Location entity
        public int Id { get; set; }

        // Name of the venue
        public string Name { get; set; } = "";

        // Optional address of the venue
        public string? Address { get; set; }

        // Navigation property: represents the one-to-many relationship between Location and Show
        // A location can host multiple shows
        public List<Show> Shows { get; set; } = new List<Show>();
    }