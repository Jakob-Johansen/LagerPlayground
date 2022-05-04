﻿using LagerPlayground.Data;
using LagerPlayground.Models;
using LagerPlayground.Models.VM;
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

        // https://software-help.shiphero.com/hc/en-us/articles/4419353992077-ShipHero-Putaway

        public async Task<IActionResult> TransferProduct()
        {
            var productLocation = await _context.Product_Locations
                .Include(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.Quantity).Where(x => x.LocationBarcode == "Receiving-Station").ToListAsync();

            int allUnits = 0;

            foreach (var item in productLocation)
            {
                if (item.Quantity != 0)
                {
                    if (allUnits == 0)
                    {
                        allUnits = item.Quantity;
                    }
                    else
                    {
                        allUnits += item.Quantity;
                    }
                }
            }

            ViewBag.AllUnits = allUnits;

            return View(productLocation);
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
        public async Task<JsonResult> TransferProduct(string scannedBarcode, string productBarcode, int quantity)
        {
            if (quantity == 0)
            {
                return Json(new { booleanError = true, msg = "No product quantity to move" });
            }

            if (scannedBarcode == null || scannedBarcode.Length == 0)
            {
                return Json(new { booleanError = true, msg = "No barcode was scanned, try again" });
            }

            var positionLocation = await _context.Locations_Positions.FirstOrDefaultAsync(x => x.FullLocationBarcode == scannedBarcode);

            if (positionLocation == null)
            {
                return Json(new { booleanError = true, locationNotFound = true, msg = "No Location barcode was found" });
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == productBarcode);

            if (product == null)
            {
                return Json(new { booleanError = true, locationNotFound = false, msg = "No product with the scanned barcode was found" });
            }

            var productLocationReceivingStation = await _context.Product_Locations.Where(x => x.LocationBarcode == "Receiving-Station").FirstOrDefaultAsync(x => x.ProductID == product.ID);

            if (productLocationReceivingStation == null || productLocationReceivingStation.Quantity < 1)
            {
                return Json(new { booleanError = true, locationNotFound = false, msg = "Receiving station has non products with the scanned barcode" });
            }

            if (productLocationReceivingStation.Quantity < quantity)
            {
                return Json(new { booleanError = true, locationNotFound = false, msg = "You can't transfer more products than the receiving station holds" });
            }

            try
            {
                int calResult = 0;
                List<Product_Locations> productLocationsList = new();

                var productLocation = await _context.Product_Locations
                    .Where(x => x.LocationBarcode == positionLocation.FullLocationBarcode)
                    .FirstOrDefaultAsync(x => x.ProductID == product.ID);

                if (productLocation == null)
                {
                    Product_Locations product_Locations = new();
                    product_Locations.ProductID = product.ID;
                    product_Locations.Created = DateTime.Now;
                    product_Locations.Quantity = quantity;
                    product_Locations.LocationBarcode = positionLocation.FullLocationBarcode;
                    product_Locations.Locations_PositionsID = positionLocation.ID;

                    _context.Product_Locations.Add(product_Locations);
                }
                else
                {
                    calResult = productLocation.Quantity + quantity;
                    if (await TryUpdateModelAsync<Product_Locations>(
                        productLocation,
                        "",
                        x => x.Quantity))
                    {
                        productLocation.Quantity = calResult;
                    }

                    productLocationsList.Add(productLocation);
                }

                calResult = productLocationReceivingStation.Quantity - quantity;

                if (await TryUpdateModelAsync<Product_Locations>(
                    productLocationReceivingStation,
                    "",
                    x => x.Quantity))
                {
                    productLocationReceivingStation.Quantity = calResult;
                }

                productLocationsList.Add(productLocationReceivingStation);

                _context.Product_Locations.UpdateRange(productLocationsList);

                await _context.SaveChangesAsync();

                return Json(new { booleanError = false });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, locationNotFound = true, msg = "An database error has occured" });
            }
        }

        public async Task<IActionResult> Putaway()
        {
            var productLocation = await _context.Product_Locations
                .Include(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.Quantity).Where(x => x.LocationBarcode == "Receiving-Station").ToListAsync();

            int allUnits = 0;

            foreach (var item in productLocation)
            {
                if (item.Quantity != 0)
                {
                    if (allUnits == 0)
                    {
                        allUnits = item.Quantity;
                    }
                    else
                    {
                        allUnits += item.Quantity;
                    }
                }
            }

            ViewBag.AllUnits = allUnits;
            return View(productLocation);
        }

        public async Task<JsonResult> PutawayGetProductFromLocation(string barcode, List<int> productsInList)
        {
            if (barcode == null)
            {
                return Json(new { booleanError = true, errorMsg = "No barcode was scanned" });
            }

            List<VMPutawayGetProductFromLocation> vmList = new();

            var getProduct = await _context.Product_Locations
                .Include(x => x.Product).Where(x => x.Product.BarcodeID == barcode)
                .Include(x => x.Locations_Positions)
                .AsNoTracking().ToListAsync();

            var getLocations = await _context.Product_Locations
                .AsNoTracking().ToListAsync();

            foreach (var productlocations in getProduct)
            {
                if (!productsInList.Contains(productlocations.ID))
                {
                    VMPutawayGetProductFromLocation vm = new();

                    int allSkus = 0;
                    int allUnits = 0;
                    foreach (var locations in getLocations.Where(x => x.Locations_PositionsID == productlocations.Locations_PositionsID))
                    {
                        allSkus++;
                        if (allUnits == 0)
                            allUnits = locations.Quantity;
                        else
                            allUnits += locations.Quantity;
                    }

                    vm.AllUnits = allUnits;
                    vm.AllSkus = allSkus;

                    vm.Product_Locations = productlocations;
                    vmList.Add(vm);
                }
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.BarcodeID == barcode);

            //vmList != null && vmList.Count != 0 && 
            if (product != null)
            {
                return Json(new { booleanError = false, productFound = true, productlocations = vmList, product });
            }

            var getProductLocation = await _context.Product_Locations
                .Include(x => x.Locations_Positions).Where(x => x.LocationBarcode == barcode)
                .Include(x => x.Product)
                .AsNoTracking().ToListAsync();

            //https://stackoverflow.com/a/27851493/12839168
            foreach (var item in getProductLocation.ToList())
            {
                if (productsInList.Contains(item.ID))
                {
                    getProductLocation.Remove(item);
                }
            }

            if (getProductLocation != null && getProductLocation.Count != 0)
            {
                return Json(new { booleanError = false, productFound = false, productlocations = getProductLocation });
            }

            return Json(new { booleanError = true, errorMsg = "No product or location was found" });
        }

        public async Task<JsonResult> PutawayGetOneProductFromLocation(int? productlocationID, List<int> productsInList)
        {
            if (productlocationID == null || productlocationID == 0)
            {
                return Json(new { booleanError = true, errorMsg = "No Product Location ID Was Found" });
            }

            var productlocations = await _context.Product_Locations
                .Include(x => x.Locations_Positions)
                .Include(x => x.Product)
                .AsNoTracking().FirstOrDefaultAsync(x => x.ID == productlocationID);

            if (productlocations == null)
            {
                return Json(new { booleanError = true, errorMsg = "No Product Location Was Found" });
            }

            if (productsInList.Contains(productlocations.ID))
            {
                return Json(new { booleanError = true, errorMsg = "Product Location already exist on the list" });
            }

            return Json(new { booleanError = false, productlocations });
        }
    }
}
