
using System.ComponentModel.DataAnnotations.Schema;


using TicketHub.Models;


public class Category
{
    // Primary key for the Category entity
    public int Id { get; set; }

    // Name of the category ("Concert", "Theater")
    public string Name { get; set; } = "";

    // Navigation property
    // A category can have multiple shows associated with it
    public List<Show> Shows { get; set; } = new List<Show>();
}