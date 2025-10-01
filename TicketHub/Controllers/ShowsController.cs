using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var ticketHubContext = _context.Set<Show>().Include(s => s.Category).Include(s => s.Location).Include(s => s.Owner);
            return View(await ticketHubContext.ToListAsync());
        }

        // GET: Shows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Set<Show>()
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // GET: Shows/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name");
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "Id", "Name");
            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate")] Show show)
        {
            // initialize CreateDate to current date and time
            show.CreateDate = DateTime.Now;

            if (ModelState.IsValid)

            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", show.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", show.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "Id", "Name", show.OwnerId);
            return View(show);
        }

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Set<Show>().FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", show.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Id", show.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "Id", "Id", show.OwnerId);
            return View(show);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate")] Show show)
        {
            if (id != show.Id)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", show.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Id", show.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Set<Owner>(), "Id", "Id", show.OwnerId);
            return View(show);
        }

        // GET: Shows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Set<Show>()
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Set<Show>().FindAsync(id);
            if (show != null)
            {
                _context.Set<Show>().Remove(show);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(int id)
        {
            return _context.Set<Show>().Any(e => e.Id == id);
        }
    }
}
