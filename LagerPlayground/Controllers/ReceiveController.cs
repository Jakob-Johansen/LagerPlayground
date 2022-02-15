using LagerPlayground.Data;
using LagerPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class ReceiveController : Controller
    {
        private readonly Context _context;
        public ReceiveController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Receive(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var receiveOrderDetails = await _context.ReceivingOrder_Details
                .Include(x => x.ReceivingOrder_Items)
                    .ThenInclude(t => t.Product)
                .FirstOrDefaultAsync(x => x.ID == ID);

            if (receiveOrderDetails == null)
            {
                return NotFound();
            }

            return View(receiveOrderDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddProduct(string barcode, int receivingOrderDetailsID)
        {
            if (barcode == null)
            {
                return Json(new {boolean = false, msg = "No barcode found"});
            }

            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcode);

            if (productToUpdate == null)
            {
                return Json(new {boolean = false, msg = "No product with this barcode was found"});
            }

            int updatedQuantity = productToUpdate.Quantity + 1;

            if (await TryUpdateModelAsync<Product>(
                productToUpdate,
                "",
                p => p.Quantity))
            {
                try
                {
                    productToUpdate.Quantity = updatedQuantity;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return Json(new { boolean = false, msg = "An database error has occured" });
                }
            }
            return Json(new {boolean = true});
        }
    }
}
