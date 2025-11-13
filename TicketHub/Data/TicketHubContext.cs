using Microsoft.EntityFrameworkCore;
using TicketHub.Models;

namespace TicketHub.Data
{
    public class TicketHubContext : DbContext
    {
        public TicketHubContext(DbContextOptions<TicketHubContext> options) : base(options) { }

        public DbSet<Show> Show { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Location> Location { get; set; } = default!;
        public DbSet<Owner> Owner { get; set; } = default!;
        public DbSet<Purchase> Purchases { get; set; } = default!;
    }
}