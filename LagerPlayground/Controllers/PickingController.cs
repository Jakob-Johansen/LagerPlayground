using LagerPlayground.Data;
using LagerPlayground.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class PickingController : Controller
    {
        private readonly Context _context;

        public PickingController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            OrdersReadyToPickCount vmOrders = new();
            vmOrders.AllOrdersNumber = _context.Order_Details.ToList().Count;

            return View(vmOrders);
        }

        public IActionResult MultiPickSite()
        {
            return View();
        }
    }
}
