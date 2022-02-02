using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LagerPlayground.Models;
using LagerPlayground.Helpers;
using LagerPlayground.Models.VM;

namespace LagerPlayground.Controllers
{
    public class WarehouseController : Controller
    {
        public readonly Context _context;
        private readonly IWebHostEnvironment _env;
        public WarehouseController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
            if (orderDetails == null)
            {
                return NotFound();
            }

            var orderItems = await _context.Order_Items.Where(x => x.Order_DetailsID == orderDetails.ID).ToListAsync();
            var custommer = await _context.Custommers.FirstOrDefaultAsync(x => x.ID == orderDetails.CustommerID);

            try
            {
                if (orderItems != null)
                {
                    _context.Order_Items.RemoveRange(orderItems);
                }

                if (custommer != null)
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

        // ---Totes---

        public async Task<IActionResult> Totes()
        {
            var totes = await _context.Totes.ToListAsync();
            return View(totes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ToteCreate(string toteName, int quantity, bool printBool)
        {
            toteName = toteName.Trim();

            if (toteName == "" || quantity < 0)
            {
                return Json(new { errorBoolean = true, errorMsg = "Check your input fields" });
            }

            var newestTote = await _context.Totes.OrderByDescending(x => x.Number).FirstOrDefaultAsync();
            int toteNumber;

            if (newestTote == null)
                toteNumber = 0;
            else
                toteNumber = newestTote.Number;

            List<Tote> toteList = new();
            List<PdfModel> pdfModelList = new();
            for (int i = 0; i < quantity; i++)
            {
                toteNumber++;
                toteList.Add(new Tote {
                    Name = toteName + "-" + toteNumber,
                    Barcode = toteNumber.ToString("D15"),
                    Number = toteNumber,
                    Created = DateTime.Now
                });

                pdfModelList.Add(new PdfModel
                {
                    Name = toteName + "-" + toteNumber,
                    Barcode = toteNumber.ToString("D15")
                });
            }

            try
            {
                await _context.Totes.AddRangeAsync(toteList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An database error has occured, try again" });
            }

            if (printBool == true)
            {
                PdfHelper pdfHelper = new(_env);
                pdfHelper.GenerateBarcodesPrint(pdfModelList);
            }

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        public async Task<JsonResult> TotePrintOneBarcode(int? ID)
        {
            if (ID == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find a tote with this ID" });
            }

            var tote = await _context.Totes.FirstOrDefaultAsync(x => x.ID == ID);

            if (tote == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find this Tote" });
            }

            PdfHelper pdfHelper = new(_env);
            pdfHelper.GererateBarcode(tote.Name, tote.Barcode);

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        public async Task<JsonResult> TotePrintSelectedBarcodes(List<int?> IDs)
        {
            if (IDs == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find the IDs" });
            }

            List<PdfModel> selectedTotes = new();

            foreach (var ID in IDs)
            {
                var tote = await _context.Totes.FirstOrDefaultAsync(x => x.ID == ID);
                if (tote != null)
                {
                    selectedTotes.Add(new PdfModel
                    {
                        Name = tote.Name,
                        Barcode = tote.Barcode
                    });
                }
            }

            PdfHelper pdfHelper = new(_env);
            pdfHelper.GenerateBarcodesPrint(selectedTotes);

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ToteDeleteOne(int? ID)
        {
            if (ID == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find the ID" });
            }

            var tote = await _context.Totes.FindAsync(ID);

            if (tote == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find a tote with this ID" });
            }

            try
            {
                _context.Totes.Remove(tote);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ToteDeleteSelected(List<int?> IDs)
        {
            if (IDs == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "No totes was selected" });
            }

            List<Tote> totesToDeleteList = new();
            foreach (int? ID in IDs)
            {
                var tote = await _context.Totes.FindAsync(ID);
                if (tote != null)
                {
                    totesToDeleteList.Add(tote);
                }
            }

            try
            {
                _context.Totes.RemoveRange(totesToDeleteList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }

            return Json(new { errorBoolean = false });
        }

        // ---ReceivingBoxes---

        public async Task<IActionResult>  ReceivingBoxes()
        {
            var receivingBoxes = await _context.ReceivingBoxes.ToListAsync();
            return View(receivingBoxes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReceivingBoxCreate(string boxName, int quantity, bool printBool)
        {
            boxName = boxName.Trim();

            if (boxName == "" || quantity < 0)
            {
                return Json(new { errorBoolean = true, errorMsg = "Check your input fields" });
            }

            var newestBox = await _context.ReceivingBoxes.OrderByDescending(x => x.Number).FirstOrDefaultAsync();
            int boxNumber;

            if (newestBox == null)
                boxNumber = 0;
            else
                boxNumber = newestBox.Number;

            List<PdfModel> pdfModelList = new();
            List<ReceivingBox> boxList = new();
            for (int i = 0; i < quantity; i++)
            {
                boxNumber++;
                boxList.Add(new ReceivingBox
                {
                    Name = boxName + "-" + boxNumber,
                    Barcode = boxNumber.ToString("D15"),
                    Number = boxNumber,
                    Created = DateTime.Now
                });

                pdfModelList.Add(new PdfModel
                {
                    Name = boxName + "-" + boxNumber,
                    Barcode = boxNumber.ToString("D15")
                });
            }

            try
            {
                await _context.ReceivingBoxes.AddRangeAsync(boxList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An database error has occured, try again" });
            }

            if (printBool == true)
            {
                PdfHelper pdfHelper = new(_env);
                pdfHelper.GenerateBarcodesPrint(pdfModelList);
            }

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        public async Task<JsonResult> ReceivingBoxPrintOneBarcode(int? ID)
        {
            if (ID == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find a tote with this ID" });
            }

            var receivingBox = await _context.ReceivingBoxes.FirstOrDefaultAsync(x => x.ID == ID);

            if (receivingBox == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find this Tote" });
            }

            PdfHelper pdfHelper = new(_env);
            pdfHelper.GererateBarcode(receivingBox.Name, receivingBox.Barcode);

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        public async Task<JsonResult> ReceivingBoxPrintSelectedBarcodes(List<int?> IDs)
        {
            if (IDs == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find the IDs" });
            }

            List<PdfModel> selectedTotes = new();

            foreach (var ID in IDs)
            {
                var receivingBoxes = await _context.ReceivingBoxes.FirstOrDefaultAsync(x => x.ID == ID);
                if (receivingBoxes != null)
                {
                    selectedTotes.Add(new PdfModel
                    {
                        Name = receivingBoxes.Name,
                        Barcode = receivingBoxes.Barcode
                    });
                }
            }

            PdfHelper pdfHelper = new(_env);
            pdfHelper.GenerateBarcodesPrint(selectedTotes);

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReceivingBoxDeleteOne(int? ID)
        {
            if (ID == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find the ID" });
            }

            var receivingBox = await _context.ReceivingBoxes.FindAsync(ID);

            if (receivingBox == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "Cant find a tote with this ID" });
            }

            try
            {
                _context.ReceivingBoxes.Remove(receivingBox);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }

            return Json(new { errorBoolean = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReceivingBoxDeleteSelected(List<int?> IDs)
        {
            if (IDs == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "No totes was selected" });
            }

            List<ReceivingBox> receivingBoxToDeleteList = new();
            foreach (int? ID in IDs)
            {
                var receivingBox = await _context.ReceivingBoxes.FindAsync(ID);
                if (receivingBox != null)
                {
                    receivingBoxToDeleteList.Add(receivingBox);
                }
            }

            try
            {
                _context.ReceivingBoxes.RemoveRange(receivingBoxToDeleteList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }

            return Json(new { errorBoolean = false });
        }
    }
}
