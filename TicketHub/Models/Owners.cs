using System.ComponentModel.DataAnnotations.Schema;
using TicketHub.Models;

public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; } = "";     // Organizer name
    public string? Email { get; set; }         // Optional email

    // Navigation property – list of shows by this owner
    public List<Show> Shows { get; set; } = new List<Show>();
}