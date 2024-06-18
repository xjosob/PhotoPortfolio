using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPortfolio.Data;
using PhotoPortfolio.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PhotoPortfolio.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PhotosController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Create a new photo
        public IActionResult Create(int albumId)
        {
            ViewBag.AlbumId = albumId;
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int albumId, IFormFile file, [Bind("Title")] Photo photo)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploadPath, file.FileName);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                photo.FilePath = $"/uploads/{file.FileName}";
                photo.AlbumID = albumId;

                _context.Add(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Albums", new { id = albumId });
            }

            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Albums", new { id = photo.AlbumID });
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoID == id);
        }
    }
}

