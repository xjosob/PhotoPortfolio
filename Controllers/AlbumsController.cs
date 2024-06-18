using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPortfolio.Data;
using PhotoPortfolio.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PhotoPortfolio.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AlbumsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Album album, IFormFile Thumbnail)
        {
            if (ModelState.IsValid)
            {
                if (Thumbnail != null && Thumbnail.Length > 0)
                {
                    var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploadPath, Thumbnail.FileName);

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Thumbnail.CopyToAsync(fileStream);
                    }

                    album.ThumbnailPath = $"/uploads/{Thumbnail.FileName}";
                }

                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumId,Title,Description,ThumbnailPath")] Album album, IFormFile Thumbnail)
        {
            if (id != album.AlbumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Thumbnail != null && Thumbnail.Length > 0)
                    {
                        var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploadPath, Thumbnail.FileName);

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Thumbnail.CopyToAsync(fileStream);
                        }

                        album.ThumbnailPath = $"/uploads/{Thumbnail.FileName}";
                    }

                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumID))
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
            return View(album);
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumID == id);
        }
    }
}
