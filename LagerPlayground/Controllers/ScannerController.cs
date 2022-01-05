using Microsoft.AspNetCore.Mvc;

namespace LagerPlayground.Controllers
{
    public class ScannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
