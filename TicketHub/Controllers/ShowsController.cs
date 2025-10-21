
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketHub.Data;
using TicketHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace TicketHub.Controllers
{
    [Authorize]

    public class ShowsController : Controller
    {
       
        private readonly TicketHubContext _context;

       
        public ShowsController(TicketHubContext context)
        {
            _context = context;
        }

        // GET: Shows
        // Retrieves and displays a list of all shows with related Category, Location, and Owner data
        public async Task<IActionResult> Index()
        {
            var shows = await _context.Show
                // Include the related Category for each Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .ToListAsync(); // Execute the query asynchronously and return the result as a list

            return View(shows); // Pass the list of shows to the view
        }

        // GET: Shows/Details/5
        // Displays detailed information for a specific show
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound(); 

            var show = await _context.Show
                // Include related Category, Location, and Owner data
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id); // Find the show with the specified ID

            if (show == null)
                return NotFound(); 

            return View(show); // Return the details view with the show data
        }

        // GET: Shows/Create
        // Displays the form to create a new show
        public IActionResult Create()
        {
            // Populate dropdown lists for Category, Location, and Owner
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name");
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name");
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name");
            return View(); 
        }

        // POST: Shows/Create
        // Handles the creation of a new show and saves it to the database
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate,ImageFile")] Show show)
        {
            show.CreateDate = DateTime.Now; // Set the creation date to the current date and time

            // Handle image upload if a file is provided
            if (show.ImageFile != null)
            {
                string fileName = Path.GetFileName(show.ImageFile.FileName); // Get the original file name
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);// Define the path to save the image

                using (var stream = new FileStream(filePath, FileMode.Create))// Create a file stream to write the image
                {
                    await show.ImageFile.CopyToAsync(stream); // Copy the uploaded file to the server
                }

                show.ImageFilename = fileName; // Store the file name in the database
            }

            if (ModelState.IsValid) 
            {
                _context.Add(show); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); 
            }

            // Re-populate dropdown lists if model validation fails
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View(show); 
        }

        // GET: Shows/Edit/5
        // Displays the form to edit an existing show
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound(); 

            var show = await _context.Show.FindAsync(id); 
            if (show == null)
                return NotFound();

            // Populate dropdown lists with current selections
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);

            return View(show); 
        }

        // POST: Shows/Edit/5
        // Handles the update of an existing show
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate,ImageFile")] Show show)
        {
            if (id != show.Id)
                return NotFound(); 

            if (ModelState.IsValid)
            {
                var existingShow = await _context.Show.FindAsync(id);
                if (existingShow == null)
                    return NotFound();

                existingShow.Title = show.Title;
                existingShow.Description = show.Description; 
                existingShow.CategoryId = show.CategoryId;
                existingShow.LocationId = show.LocationId;
                existingShow.OwnerId = show.OwnerId;
                existingShow.Date = show.Date;
                existingShow.Time = show.Time;
                existingShow.CreateDate = show.CreateDate;

                if (show.ImageFile != null)// Handle image upload if a new file is provided
                {
                    string fileName = Path.GetFileName(show.ImageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await show.ImageFile.CopyToAsync(stream);
                    }

                    existingShow.ImageFilename = fileName;
                }

                _context.Update(existingShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // It will re-populate dropdown lists if model validation fails
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View(show); 
        }

        // GET: Shows/Delete/5
        // Displays the confirmation page to delete a show
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound(); 

            var show = await _context.Show
                // Include related Category, Location, and Owner data
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id); 

            if (show == null)
                return NotFound(); 

            return View(show); 
        }

        // POST: Shows/Delete/5
        // Handles the deletion of a show after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Show.FindAsync(id);
            if (show != null)
            {
                // ✅ Delete image file from wwwroot/images
                if (!string.IsNullOrEmpty(show.ImageFilename))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", show.ImageFilename);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Show.Remove(show);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
