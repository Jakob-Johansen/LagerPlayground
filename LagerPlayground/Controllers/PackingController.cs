using Microsoft.AspNetCore.Mvc;

namespace LagerPlayground.Controllers
{
    public class PackingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pack()
        {
            return View();
        }
    }
}
