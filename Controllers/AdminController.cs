using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPortfolio.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PhotoPortfolio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var albums = await _context.Albums.Include(a => a.Photos).ToListAsync();
            return View(albums);
        }

    }
}
