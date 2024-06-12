// Controllers/AlbumsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPortfolio.Data;
using PhotoPortfolio.Models;
using System.Threading.Tasks;

namespace PhotoPortfolio.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all albums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Albums.ToListAsync());
        }

        // Details of a specific album
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var album = await _context.Albums
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(m => m.AlbumID == id);

            if (album == null) return NotFound();

            return View(album);
        }

        // Create a new album
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }
    }
}
