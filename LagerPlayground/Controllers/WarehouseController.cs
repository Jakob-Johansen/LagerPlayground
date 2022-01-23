using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class WarehouseController : Controller
    {
        public readonly Context _context;
        public WarehouseController(Context context)
        {
            _context = context;
        }

        // ---Orders---

        public async Task<IActionResult> AllOrders()
        {
            var orderDetails = await _context.Order_Details
            .Include(s => s.Order_Items)
            .ThenInclude(e => e.Product)
            .Include(s => s.Custommer)
            .AsNoTracking().ToListAsync();
            return View(orderDetails);
        }

        // ---Totes---

        public IActionResult Totes()
        {
            return View();
        }
    }
}
