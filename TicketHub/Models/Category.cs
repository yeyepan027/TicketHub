using System.ComponentModel.DataAnnotations.Schema;
using TicketHub.Models;

public class Category

{
  
    public int Id { get; set; }             // Primary key
    public string Name { get; set; } = "";  // Event type

    // Navigation property – list of shows in this category
    public List<Show> Shows { get; set; } = new List<Show>();
}