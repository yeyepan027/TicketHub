
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore;
using TicketHub.Data;
using Microsoft.AspNetCore.Authorization;

namespace TicketHub.Controllers
{
    [Authorize]
    public class LocationsController : Controller
    {
       
        private readonly TicketHubContext _context;

       
        public LocationsController(TicketHubContext context)
        {
            _context = context; 
        }

        // GET: Locations
        // Displays a list of all locations
        public async Task<IActionResult> Index()
        {
            // Retrieve all locations from the database asynchronously and pass to the view
            return View(await _context.Location.ToListAsync());
        }

        // GET: Locations/Details/5
        // Displays details of a specific location by ID
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the location with the matching ID
            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.Id == id);

            // Check if the location was found
            if (location == null)
            {
                return NotFound(); 
            }

            // Return the details view with the found location
            return View(location);
        }

        // GET: Locations/Create
        // Displays the form to create a new location
        public IActionResult Create()
        {
            return View(); // Return the create view
        }

        // POST: Locations/Create
        // Handles the submission of the create form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] Location location)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(location); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); 
            }

            // If model is invalid, return the view with validation errors
            return View(location);
        }

        // GET: Locations/Edit/5
        // Displays the form to edit an existing location
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the location by ID
            var location = await _context.Location.FindAsync(id);

            // Check if the location exists
            if (location == null)
            {
                return NotFound(); 
            }

            // Return the edit view with the location data
            return View(location);
        }

        // POST: Locations/Edit/5
        // Handles the submission of the edit form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Location location)
        {
            // Check if the provided ID matches the location's ID
            if (id != location.Id)
            {
                return NotFound(); 
            }

           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location); 
                    await _context.SaveChangesAsync(); 
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues
                {
                    // Check if the location still exists
                    if (!LocationExists(location.Id))
                    {
                        return NotFound(); 
                    }
                    else
                    {
                        throw; // Rethrow the exception if it's another issue
                    }
                }

                
                return RedirectToAction(nameof(Index));
            }

            
            return View(location);
        }

        // GET: Locations/Delete/5
        // Displays the confirmation page to delete a location
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Find the location by ID
            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.Id == id);

            // Check if the location exists
            if (location == null)
            {
                return NotFound(); 
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        // Handles the deletion of a location after confirmation
        [HttpPost, ActionName("Delete")] // Specifies the action name for the POST method
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the location by ID
            var location = await _context.Location.FindAsync(id);

            // If location exists, remove it from the context
            if (location != null)
            {
                _context.Location.Remove(location);
            }
           
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a location exists by ID
        private bool LocationExists(int id)
        {
            // Returns true if any location matches the given ID
            return _context.Location.Any(e => e.Id == id);
        }
    }
}
