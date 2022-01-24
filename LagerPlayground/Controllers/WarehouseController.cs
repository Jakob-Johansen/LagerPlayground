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

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.Order_Details
                .Include(x => x.Order_Items)
                .ThenInclude(c => c.Product)
                .Include(x => x.Custommer)
                .AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // ---Totes---

        public async Task<IActionResult> Totes()
        {
            var totes = await _context.Totes.ToListAsync();
            return View(totes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateTote(string name, int quantity, int startFrom)
        {
            int number = 0;
            string barcode = name.ToUpper().Trim() + "-";
            List<string> barcodeList = new();
            for (int i = 0; i < quantity; i++)
            {
                number++;
                barcodeList.Add(barcode + startFrom++);
            }

            return Json(new { name, barcodeList });
        }
    }
}
