using LagerPlayground.Data;
using LagerPlayground.Models;
using LagerPlayground.Models.VM;
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

            ReceiveSite receiveSite = new();
            receiveSite.ReceivingOrder_Details = await _context.ReceivingOrder_Details
                .Include(x => x.ReceivingOrder_Items)
                    .ThenInclude(t => t.Product)
                .Include(x => x.ReceivingOrder_Items)
                    .ThenInclude(t => t.ReceiveRejecteds)
                .FirstOrDefaultAsync(x => x.ID == ID);

            receiveSite.ReceiveRejectedReasons = await _context.ReceiveRejectedReasons.ToListAsync();

            if (receiveSite.ReceivingOrder_Details == null)
            {
                return NotFound();
            }

            int allOrderedProducts = 0;
            int allAcceptedProducts = 0;

            foreach (var item in receiveSite.ReceivingOrder_Details.ReceivingOrder_Items)
            {
                allOrderedProducts += item.Quantity;
                allAcceptedProducts += item.Accepted;
            }

            ViewData["AllOrderedProducts"] = allOrderedProducts;
            ViewData["allAcceptedProducts"] = allAcceptedProducts;

            return View(receiveSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddProduct(string barcode, int receivingItemID)
        {
            if (barcode == null)
            {
                return Json(new {boolean = false, msg = "No barcode found"});
            }

            var receiveOrderItemToUpdate = await _context.ReceivingOrder_Items.FirstOrDefaultAsync(x => x.ID == receivingItemID);
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcode);

            if (receiveOrderItemToUpdate == null || productToUpdate == null)
            {
                return Json(new {boolean = false, msg = "No product with this barcode was found"});
            }

            var tryUpdateOrderItem = await TryUpdateModelAsync<ReceivingOrder_Items>(
            receiveOrderItemToUpdate, "", x => x.Accepted);

            var tryUpdateProduct = await TryUpdateModelAsync<Product>(
                productToUpdate, "", x => x.Quantity);

            int updatedQuantity = productToUpdate.Quantity + 1;
            int updateAccepted = receiveOrderItemToUpdate.Accepted + 1;

            if (tryUpdateOrderItem == true && tryUpdateProduct == true)
            {
                try
                {
                    receiveOrderItemToUpdate.Accepted = updateAccepted;
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

        [HttpGet]
        public async Task<JsonResult> GetOneReceiveOrder(string barcode)
        {
            if (barcode == null)
            {
                return Json(new { boolean = false, msg = "No barcode was found" });
            }

            var receiveOrder = await _context.ReceivingOrder_Items
                .Include(x => x.Product)
                .Include(x => x.ReceiveRejecteds)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Product.BarcodeID == barcode);

            if (receiveOrder == null)
            {
                return Json(new { boolean = false, msg = "No product with this barcode was found" });
            }

            return Json(new { boolean = true, receiveorder = receiveOrder});
        }

        //---Reject Reasons---

        public async Task<IActionResult> RejectReasons()
        {
            var rejectReasons = await _context.ReceiveRejectedReasons.ToListAsync();

            return View(rejectReasons);
        }

        public IActionResult CreateRejectReason()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRejectReason([Bind("Reason")] ReceiveRejectedReasons receiveRejectedReasons)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    receiveRejectedReasons.Created = DateTime.Now;
                    _context.ReceiveRejectedReasons.Add(receiveRejectedReasons);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RejectReasons");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(receiveRejectedReasons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRejectReason(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("RejectReasons");
            }

            var rejectReason = await _context.ReceiveRejectedReasons.FindAsync(ID);

            if (rejectReason == null)
            {
                return RedirectToAction("RejectReasons");
            }

            try
            {
                _context.ReceiveRejectedReasons.Remove(rejectReason);
                await _context.SaveChangesAsync();
                return RedirectToAction("RejectReasons");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("RejectReasons");
            }
        }
    }
}
