using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LagerPlayground.Models;

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

        // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument.print?view=dotnet-plat-ext-6.0

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateTote(string toteName, int quantity, bool printBool)
        {
            toteName = toteName.Trim();
            printBool = false;

            if (toteName == "" || quantity < 0)
            {
                return Json(new { errorBoolean = true, exceptionError = false, errorMsg = "Check your input fields" });
            }

            var newestTote = await _context.Totes.OrderByDescending(x => x.Number).FirstOrDefaultAsync();
            int toteNumber;

            if (newestTote == null)
                toteNumber = 0;
            else
                toteNumber = newestTote.Number;

            List<Tote> toteList = new();
            for (int i = 0; i < quantity; i++)
            {
                toteNumber++;
                toteList.Add(new Tote {
                    Name = toteName,
                    Barcode = "T-" + toteNumber.ToString("D13"),
                    Number = toteNumber,
                    Created = DateTime.Now
                });
            }

            string saveVal;
            try
            {
                await _context.AddRangeAsync(toteList);
                await _context.SaveChangesAsync();
                saveVal = "Saved";
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, exceptionError = true, errorMsg = "An database error has occured, try again" });
            }

            //if (printBool == true)
            //{
            //    // Create pdf
            //}

            return Json(new { errorBoolean = false, name = toteName, toteList, saveVal });
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
