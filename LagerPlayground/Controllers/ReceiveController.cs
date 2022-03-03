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
            // For Testing
            //Thread.Sleep(1000);

            if (barcode == null)
            {
                return Json(new {boolean = false, msg = "No barcode was scanned, try again"});
            }

            var receiveOrderItemToUpdate = await _context.ReceivingOrder_Items.FirstOrDefaultAsync(x => x.ID == receivingItemID);
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcode);

            if (receiveOrderItemToUpdate == null || productToUpdate == null)
            {
                return Json(new {boolean = false, msg = "There was no product that matched barcode " + barcode });
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
                    // For Testing
                    //throw new DbUpdateException();

                    receiveOrderItemToUpdate.Accepted = updateAccepted;
                    productToUpdate.Quantity = updatedQuantity;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return Json(new { boolean = false, databaseError = true, msg = "An database error has occured, try again or contact an administrator" });
                }
            }

            return Json(new {boolean = true});
        }

        [HttpGet]
        public async Task<JsonResult> GetOneReceiveOrder(string barcode)
        {
            // For Testing
            //Thread.Sleep(1000);

            if (barcode == null)
            {
                return Json(new { boolean = false, msg = "No barcode was found" });
            }

            var receiveOrder = await _context.ReceivingOrder_Items
                .Include(x => x.Product)
                .Include(x => x.ReceiveRejecteds)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Product.BarcodeID == barcode);

            var allRejectsInItem = await _context.ReceiveRejecteds
                .Include(x => x.ReceiveRejectedReasons)
                .AsNoTracking()
                .Where(x => x.ReceivingOrder_ItemsID == receiveOrder.ID).ToListAsync();

            if (receiveOrder == null)
            {
                return Json(new { boolean = false, msg = "No product with this barcode was found" });
            }

            return Json(new 
            { 
                boolean = true,
                name = receiveOrder.Product.Name,
                barcode = receiveOrder.Product.BarcodeID,
                image = receiveOrder.Product.Image,
                rejected = receiveOrder.Rejected,
                rejectedItems = allRejectsInItem,
                itemID = receiveOrder.ID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteOneRejected(int? rejectID)
        {
            if (rejectID == null)
            {
                return Json(new { boolean = false, msg = "No ID was found" });
            }

            var reject = await _context.ReceiveRejecteds.FindAsync(rejectID);

            if (reject == null)
            {
                return Json(new { boolean = false, msg = "No rejected item was found" });
            }

            int quantity = reject.Quantity;

            try
            {
                _context.ReceiveRejecteds.Remove(reject);
                await _context.SaveChangesAsync();
                return Json(new { boolean = true, quantity });
            }
            catch (DbUpdateException)
            {
                return Json(new { boolean = false, msg = "An database error has occured" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateOneReject(int? quantity, int? selectedReasonID, string note, int? itemID)
        {
            if (quantity == null)
            {
                return Json(new { boolean = false, msg = "Quantity is NULL" });
            }

            if (selectedReasonID == null)
            {
                return Json(new { boolean = false, msg = "ReasonID is NULL" });
            }

            if (itemID == null)
            {
                return Json(new { boolean = false, msg = "ItemID is NULL" });
            }

            try
            {
                ReceiveRejected receiveRejected = new();
                receiveRejected.Created = DateTime.Now;
                receiveRejected.Quantity = (int)quantity;
                receiveRejected.ReceiveRejectedReasonsID = (int)selectedReasonID;
                receiveRejected.Note = note;
                receiveRejected.ReceivingOrder_ItemsID = (int)itemID;

                _context.ReceiveRejecteds.Add(receiveRejected);
                await _context.SaveChangesAsync();
                return Json(new { boolean = true, rejectID = receiveRejected.ID });
            }
            catch (DbUpdateException)
            {
                return Json(new { boolean = false, msg = "An database error has occured" });
            }

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
