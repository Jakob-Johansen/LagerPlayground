using Microsoft.AspNetCore.Mvc;

namespace LagerPlayground.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
