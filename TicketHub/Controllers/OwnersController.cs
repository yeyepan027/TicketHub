
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
    public class OwnersController : Controller
    {
       
        private readonly TicketHubContext _context;

        
        public OwnersController(TicketHubContext context)
        {
            _context = context; // Assign the injected context to the private field
        }

        // GET: Owners
        // Displays a list of all owners
        public async Task<IActionResult> Index()
        {
            // Retrieve all owners from the database asynchronously and pass to the view
            return View(await _context.Owner.ToListAsync());
        }

        // GET: Owners/Details/5
        // Displays details of a specific owner by ID
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the owner with the matching ID
            var owner = await _context.Owner
                .FirstOrDefaultAsync(m => m.Id == id);

            // Check if the owner was found
            if (owner == null)
            {
                return NotFound(); 
            }

            
            return View(owner);
        }

        // GET: Owners/Create
        // Displays the form to create a new owner
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: Owners/Create
        // Handles the submission of the create form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] Owner owner)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(owner); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }

            // If model is invalid, return the view with validation errors
            return View(owner);
        }

        // GET: Owners/Edit/5
        // Displays the form to edit an existing owner
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the owner by ID
            var owner = await _context.Owner.FindAsync(id);

            // Check if the owner exists
            if (owner == null)
            {
                return NotFound(); 
            }

            return View(owner);
        }

        // POST: Owners/Edit/5
        // Handles the submission of the edit form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Owner owner)
        {
            // Check if the provided ID matches the owner's ID
            if (id != owner.Id)
            {
                return NotFound(); 
            }

            // Check if the submitted model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(owner); 
                    await _context.SaveChangesAsync(); 
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues
                {
                    // Check if the owner still exists
                    if (!OwnerExists(owner.Id))
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

            // If model is invalid, return the view with validation errors
            return View(owner);
        }

        // GET: Owners/Delete/5
        // Displays the confirmation page to delete an owner
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the owner by ID
            var owner = await _context.Owner
                .FirstOrDefaultAsync(m => m.Id == id);

            // Check if the owner exists
            if (owner == null)
            {
                return NotFound(); 
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        // Handles the deletion of an owner after confirmation
        [HttpPost, ActionName("Delete")] // Specifies the action name for the POST method
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the owner by ID
            var owner = await _context.Owner.FindAsync(id);

            // If owner exists, remove it from the context
            if (owner != null)
            {
                _context.Owner.Remove(owner);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an owner exists by ID
        private bool OwnerExists(int id)
        {
            // Returns true if any owner matches the given ID
            return _context.Owner.Any(e => e.Id == id);
        }
    }
}