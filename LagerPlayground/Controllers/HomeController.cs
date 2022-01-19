using LagerPlayground.Data;
using LagerPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LagerPlayground.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        //public const string SessionKeyName = "_Name";
        //public const string SessionKeyAge = "_Age";

        // https://stackoverflow.com/questions/46921275/access-session-variable-in-razor-view-net-core-2
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
        // https://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            //{
            //    HttpContext.Session.SetString(SessionKeyName, "Jakob");
            //    HttpContext.Session.SetInt32(SessionKeyAge, 21);
            //}

            return View();
        }

        public IActionResult ScannerFun()
        {
            return View();
        }

        // Arrivals
        public IActionResult Arrivals()
        {
            return View();
        }

        //public IActionResult 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}