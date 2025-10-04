using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketHub.Data;

namespace TicketHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly TicketHubContext _context;

        public HomeController(TicketHubContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shows = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .ToListAsync();

            return View(shows);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}