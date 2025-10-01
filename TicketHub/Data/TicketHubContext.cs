using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketHub.Models;

namespace TicketHub.Data
{
    public class TicketHubContext : DbContext
    {
        public TicketHubContext (DbContextOptions<TicketHubContext> options)
            : base(options)
        {
        }

        public DbSet<TicketHub.Models.Show> Show { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Location> Location { get; set; } = default!;
        public DbSet<Owner> Owner { get; set; } = default!;
        public object Categories { get; internal set; }
        public object Shows { get; internal set; }
    }
}
