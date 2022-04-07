using LagerPlayground.Data;
using LagerPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class TransferController : Controller
    {
        private readonly Context _context;

        public TransferController(Context context)
        {
            _context = context;
        }

        public IActionResult TransferProduct()
        {
            return View();
        }

        public async Task<JsonResult> GetNonLocationProduct(string scannedBarcode)
        {
            if (scannedBarcode.Length == 0 || scannedBarcode == null)
            {
                return Json(new { booleanError = true, msg = "No barcode was scanned, try again" });
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == scannedBarcode);

            if (product == null)
            {
                return Json(new { booleanError = true, msg = "No product with the scanned barcode was found" });
            }

            var productLocation = await _context.Product_Locations.Where(x => x.LocationBarcode == "Receiving-Station").FirstOrDefaultAsync(x => x.ProductID == product.ID);

            if (productLocation == null || productLocation.Quantity < 1)
            {
                return Json(new { booleanError = true, msg = "Receiving station has non products with the scanned barcode" });
            }

            return Json(new { booleanError = false, productName = product.Name, productBarcode = product.BarcodeID, productBrandName = product.BrandName, productCategory = product.Category, productImage = product.Image, receivingstationQuantity = productLocation.Quantity });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> TransferProduct(string scannedBarcode, string productBarcode)
        {
            if (scannedBarcode == null || scannedBarcode.Length == 0)
            {
                return Json(new { booleanError = true, msg = "No barcode was scanned, try again" });
            }

            var positionLocation = await _context.Locations_Positions.FirstOrDefaultAsync(x => x.FullLocationBarcode == scannedBarcode);

            if (positionLocation == null)
            {
                return Json(new { booleanError = true, msg = "No Location barcode was found" });
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == productBarcode);

            if (product == null)
            {
                return Json(new { booleanError = true, msg = "No product with the scanned barcode was found" });
            }

            var productLocationReceivingStation = await _context.Product_Locations.Where(x => x.LocationBarcode == "Receiving-Station").FirstOrDefaultAsync(x => x.ProductID == product.ID);

            if (productLocationReceivingStation == null || productLocationReceivingStation.Quantity < 1)
            {
                return Json(new { booleanError = true, msg = "Receiving station has non products with the scanned barcode" });
            }

            try
            {
                List<Product_Locations> productLocationsList = new();

                var productLocation = await _context.Product_Locations
                    .Where(x => x.LocationBarcode == positionLocation.FullLocationBarcode)
                    .FirstOrDefaultAsync(x => x.ProductID == product.ID);

                if (productLocation == null)
                {
                    Product_Locations product_Locations = new();
                    product_Locations.ProductID = product.ID;
                    product_Locations.Created = DateTime.Now;
                    product_Locations.Quantity = 1;
                    product_Locations.LocationBarcode = positionLocation.FullLocationBarcode;

                    _context.Product_Locations.Add(product_Locations);
                }
                else
                {
                    if (await TryUpdateModelAsync<Product_Locations>(
                        productLocation,
                        "",
                        x => x.Quantity))
                    {
                        productLocation.Quantity++;
                    }

                    productLocationsList.Add(productLocation);
                }

                if (await TryUpdateModelAsync<Product_Locations>(
                    productLocationReceivingStation,
                    "",
                    x => x.Quantity))
                {
                    productLocationReceivingStation.Quantity--;
                }

                productLocationsList.Add(productLocationReceivingStation);

                _context.Product_Locations.UpdateRange(productLocationReceivingStation);

                await _context.SaveChangesAsync();

                return Json(new { booleanError = false });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, msg = "An database error has occured" });
            }
        }
    }
}
