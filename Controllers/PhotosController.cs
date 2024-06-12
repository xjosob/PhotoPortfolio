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
        private readonly IWebHostEnvironment _enviroment;

        public PhotosController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }
        public IActionResult Create(int albumId)
        {
            ViewBag.AlbumId = albumId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int albumId, IFormFile file, [Bind("Title")] Photo photo)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploadPath, file.FileName);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using(var fileStream = new FileStream(filePath, FileMode.Create))
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
    }
}
