
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using TicketHub.Data;
using Microsoft.AspNetCore.Authorization;


namespace TicketHub.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      
        private readonly TicketHubContext _context;

     
        public HomeController(TicketHubContext context)
        {
            _context = context; // Assign the injected context to the private field
        }

        // GET: Home/Index
        // Displays the homepage with a list of shows including related data
        public async Task<IActionResult> Index()
        {
            // Retrieve all shows from the database, including related Category, Location, and Owner data
            var shows = await _context.Show
                .Include(s => s.Category) 
                .Include(s => s.Location)
                .Include(s => s.Owner)    
                .ToListAsync();           // Convert the result to a list asynchronously

            // Pass the list of shows to the view
            return View(shows);
        }
        public IActionResult Privacy()
        {
            return View(); // Return the Privacy view
        }
    }
}
