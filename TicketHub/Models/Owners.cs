using System.ComponentModel.DataAnnotations.Schema;
using TicketHub.Models;

public class Owner
{

    // Primary key for the Owner entity
    public int Id { get; set; }

    // Name of the organizer or owner
    public string Name { get; set; } = "";

    // Optional email address of the owner
    public string? Email { get; set; }

    // Navigation property: represents the one-to-many relationship between Owner and Show
    // An owner can organize multiple shows
    public List<Show> Shows { get; set; } = new List<Show>();
}
