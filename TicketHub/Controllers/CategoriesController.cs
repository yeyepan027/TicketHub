
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
    public class CategoriesController : Controller
    {
        private readonly TicketHubContext _context;

        
        public CategoriesController(TicketHubContext context)
        {
            _context = context; // Assign the injected context to the private field
        }

        // GET: Categories
        // Displays a list of all categories
        public async Task<IActionResult> Index()
        {
            // Retrieve all categories from the database asynchronously and pass to the view
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        // Displays details of a specific category by ID
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound(); 
            }

            // Find the category with the matching ID
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);

            
            if (category == null)
            {
                return NotFound(); 
            }

            
            return View(category);
        }

        // GET: Categories/Create
        // Displays the form to create a new category
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: Categories/Create
        // Handles the submission of the create form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(category); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); 
            }

            
            return View(category);
        }

        // GET: Categories/Edit/5
        // Displays the form to edit an existing category
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound(); 
            }

            
            var category = await _context.Category.FindAsync(id);

            // Check if the category exists
            if (category == null)
            {
                return NotFound(); 
            }

            
            return View(category);
        }

        // POST: Categories/Edit/5
        // Handles the submission of the edit form
        // Protects against overposting attacks by binding only specific properties
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            // Check if the provided ID matches the category's ID
            if (id != category.Id)
            {
                return NotFound(); 
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category); 
                    await _context.SaveChangesAsync(); 
                }
                
                catch (DbUpdateConcurrencyException) 
                {
                    
                    if (!CategoryExists(category.Id))
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

            return View(category);
        }

        // GET: Categories/Delete/5
        // Displays the confirmation page to delete a category
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound(); 
            }

            
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);

            
            if (category == null)
            {
                return NotFound(); 
            }

            // Return the delete confirmation view
            return View(category);
        }

        // POST: Categories/Delete/5
        // Handles the deletion of a category after confirmation
        [HttpPost, ActionName("Delete")] // Specifies the action name for the POST method
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the category by ID
            var category = await _context.Category.FindAsync(id);

          
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a category exists by ID
        private bool CategoryExists(int id)
        {
            // Returns true if any category matches the given ID
            return _context.Category.Any(e => e.Id == id);
        }
    }
}