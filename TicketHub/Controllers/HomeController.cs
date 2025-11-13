using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketHub.Data;
using TicketHub.Models;
using Microsoft.AspNetCore.Authorization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TicketHub.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TicketHubContext _context;
        private readonly BlobContainerClient _containerClient;

        public HomeController(TicketHubContext context, IConfiguration config)
        {
            _context = context;

            // ✅ Azure Blob Storage setup
            var connectionString = config.GetConnectionString("AppAzureStorage");
            var containerName = "tickethub-uploads"; 
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var shows = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .Include(s => s.Purchases) // ✅ Add this
                .ToListAsync();

            return View(shows);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name");
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name");
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name");
            return View("~/Views/Shows/Create.cshtml");
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate,ImageFile")] Show show)
        {
            show.CreateDate = DateTime.Now;

            if (show.ImageFile != null)
            {
                // ✅ Upload to Azure Blob Storage
                var blobClient = _containerClient.GetBlobClient(show.ImageFile.FileName);
                using (var stream = show.ImageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = show.ImageFile.ContentType });
                }

                // ✅ Save Blob URL to database
                show.ImageFilename = blobClient.Uri.ToString();
            }

            if (ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View("~/Views/Shows/Create.cshtml", show);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var show = await _context.Show.FindAsync(id);
            if (show == null) return NotFound();

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);

            return View("~/Views/Shows/Edit.cshtml", show);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,LocationId,OwnerId,Date,Time,CreateDate,ImageFile")] Show show)
        {
            if (id != show.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingShow = await _context.Show.FindAsync(id);
                if (existingShow == null) return NotFound();

                existingShow.Title = show.Title;
                existingShow.Description = show.Description;
                existingShow.CategoryId = show.CategoryId;
                existingShow.LocationId = show.LocationId;
                existingShow.OwnerId = show.OwnerId;
                existingShow.Date = show.Date;
                existingShow.Time = show.Time;
                existingShow.CreateDate = show.CreateDate;

                if (show.ImageFile != null)
                {
                    // ✅ Upload new image to Azure Blob Storage
                    var blobClient = _containerClient.GetBlobClient(show.ImageFile.FileName);
                    using (var stream = show.ImageFile.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = show.ImageFile.ContentType });
                    }

                    existingShow.ImageFilename = blobClient.Uri.ToString();
                }

                _context.Update(existingShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Name", show.CategoryId);
            ViewBag.LocationList = new SelectList(_context.Location, "Id", "Name", show.LocationId);
            ViewBag.OwnerList = new SelectList(_context.Owner, "Id", "Name", show.OwnerId);
            return View("~/Views/Shows/Edit.cshtml", show);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var show = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .Include(s => s.Purchases) // ✅ Add this
                .FirstOrDefaultAsync(m => m.Id == id);

            if (show == null) return NotFound();

            return View("~/Views/Shows/Details.cshtml", show);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var show = await _context.Show
                .Include(s => s.Category)
                .Include(s => s.Location)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (show == null) return NotFound();

            return View("~/Views/Shows/Delete.cshtml", show);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Show.FindAsync(id);
            if (show != null)
            {
                // ✅ Delete from Azure Blob Storage
                if (!string.IsNullOrEmpty(show.ImageFilename))
                {
                    var blobName = Path.GetFileName(show.ImageFilename);
                    var blobClient = _containerClient.GetBlobClient(blobName);
                    await blobClient.DeleteIfExistsAsync();
                }

                _context.Show.Remove(show);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}