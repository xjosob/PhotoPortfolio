// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;

namespace PhotoPortfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

