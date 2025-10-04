using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketHub.Data;
using TicketHub.Models;

namespace TicketHub.Controllers
{
    public class ShowsController : Controller
    {
        private readonly TicketHubContext _context;

        public ShowsController(TicketHubContext context)
        {
            _context = context;
        }

        // GET: Shows
        public async Task<IActionResult> Index()
        {
            var shows = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .ToListAsync();

            return View(shows);
        }

        // GET: Shows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var show = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (show == null)
                return NotFound();

            return View(show);
        }

        // GET: Shows/Create
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name");
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name");
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name");
            return View();
        }

        // POST: Shows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,ImageUrl,CreateDate")] Show show)
        {
            show.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View(show);
        }

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var show = await _context.Show.FindAsync(id);
            if (show == null)
                return NotFound();

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);

            return View(show);
        }

        // POST: Shows/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,ImageUrl,CreateDate")] Show show)
        {
            if (id != show.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(show);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(show.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View(show);
        }

        // GET: Shows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var show = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (show == null)
                return NotFound();

            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Show.FindAsync(id);
            if (show != null)
            {
                _context.Show.Remove(show);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(int id)
        {
            return _context.Show.Any(e => e.Id == id);
        }
    }
}