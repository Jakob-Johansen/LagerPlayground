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
        public async Task<JsonResult> CreateTote(string toteName, int quantity, int startFrom)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument.print?view=dotnet-plat-ext-6.0
            toteName = toteName.Trim();

            if (toteName == "" || quantity < 0 || startFrom < 0)
            {
                return Json(new { errorBoolean = true, exceptionError = false, errorMsg = "Check your input fields" });
            }

            int number = 0;
            string barcode = toteName.ToUpper().Trim() + "-";
            List<string> barcodeList = new();
            for (int i = 0; i < quantity; i++)
            {
                number++;
                barcodeList.Add(barcode + startFrom++);
            }

            string saveVal;
            try
            {
                //await _context.AddRangeAsync(barcodeList);
                //await _context.SaveChangesAsync();
                saveVal = "Saved";
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, exceptionError = true, errorMsg = "An database error has occured, try again" });
            }

            return Json(new { errorBoolean = false, name = toteName, barcodeList, saveVal });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // https://entityframework.net/delete-multiple-entities

            var orderDetails = await _context.Order_Details.FindAsync(id);
            if(orderDetails == null)
            {
                return NotFound();
            }

            var orderItems = await _context.Order_Items.Where(x => x.Order_DetailsID == orderDetails.ID).ToListAsync();
            var custommer = await _context.Custommers.FirstOrDefaultAsync(x => x.ID == orderDetails.CustommerID);

            try
            {
                if(orderItems != null)
                {
                    _context.Order_Items.RemoveRange(orderItems);
                }
                
                if(custommer != null)
                {
                    _context.Custommers.Remove(custommer);
                }

                _context.Order_Details.Remove(orderDetails);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return NotFound();
            }

            return RedirectToAction("AllOrders");
        }
    }
}
