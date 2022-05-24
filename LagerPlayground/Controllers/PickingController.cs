using Microsoft.AspNetCore.Mvc;

namespace LagerPlayground.Controllers
{
    public class PickingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
