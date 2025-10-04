using System.ComponentModel.DataAnnotations.Schema;
using TicketHub.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = "";     // Venue name
    public string? Address { get; set; }       // Optional address

    // Navigation property – list of shows in this location
    public List<Show> Shows { get; set; } = new List<Show>();
}