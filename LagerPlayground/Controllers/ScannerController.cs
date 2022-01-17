using LagerPlayground.Data;
using LagerPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class ScannerController : Controller
    {
        private readonly Context _context;
        public ScannerController(Context context)
        {
            _context = context;
        }

        // Scanner Fun
        public async Task<JsonResult> GetProduct(string barcodeID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcodeID);

            if (product == null)
            {
                return Json(new { boolean = false });
            }

            return Json(new { boolean = true, name = product.Name, barcodeID = product.BarcodeID, brandName = product.BrandName, category = product.Category, image = product.Image });
        }

        // Arrivals
        public async Task<JsonResult> BarcodeExist(int? id, string barcodeID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcodeID);

            bool returnBoolean = false;
            string message = "";

            if (product != null)
            {
                if (product.ID == id)
                {
                    returnBoolean = true;
                    message = "The same product";
                }
                else
                {
                    returnBoolean = false;
                    message = "This product already exist";
                }
            }
            else
            {
                returnBoolean = true;
                message = "This product does not exist";
            }

            return Json(new { boolean = returnBoolean, msg = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddMoreStock(string barcodeID, int Quantity)
        {
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcodeID);

            if (productToUpdate == null)
            {
                return Json(new { boolean = false, exception = false, msg = "No product with the scanned barcode was found" });
            }

            int newQuantity = productToUpdate.Quantity + Quantity;

            if (await TryUpdateModelAsync<Product>(
                productToUpdate,
                "",
                p => p.Quantity))
            {
                try
                {
                    productToUpdate.Quantity = newQuantity;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return Json(new { boolean = false, exception = true, msg = "An database error has occurred, try again or contact support" });
                }
            }

            return Json(new { boolean = true, exception = false, msg = Quantity.ToString() + " products has been added!" });
        }
    }
}
