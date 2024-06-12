using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPortfolio.Data;
using System.Threading.Tasks;

namespace PhotoPortfolio.Controllers
{
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
            return View();
        }
    }
}
