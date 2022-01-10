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
        public async Task<JsonResult> GetProduct(string productID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productID);

            if (product == null)
            {
                return Json(false);
            }

            return Json(product);
        }

        // Arrivals
        public async Task<JsonResult> BarcodeExist(int? id, string productID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productID);

            JsonEditModel jsonEditModel = new();

            if (product != null)
            {
                if (product.ID == id)
                {
                    jsonEditModel.Boolean = true;
                    jsonEditModel.Msg = "The same product";
                }
                else
                {
                    jsonEditModel.Boolean = false;
                    jsonEditModel.Msg = "This product already exist";
                }
            }
            else
            {
                jsonEditModel.Boolean = true;
                jsonEditModel.Msg = "This product does not exist";
            }

            return Json(jsonEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddMoreStock(string productID, int Quantity)
        {
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productID);

            int newQuantity = productToUpdate.Quantity + Quantity;

            if (productToUpdate == null)
            {
                return Json(false);
            }

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
                    return Json(new { boolean = false, msg = "An database error has occurred, try again or contact support" });
                }
            }

            return Json(new { boolean = true, msg = "" });
        }
    }
}
