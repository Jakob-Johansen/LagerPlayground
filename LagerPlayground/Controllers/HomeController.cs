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

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orderDetails = await _context.Order_Details
                .Include(s => s.Order_Items)
                .ThenInclude(e => e.Product)
                .Include(s => s.Custommer)
                .AsNoTracking().ToListAsync();
            return View(orderDetails);
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