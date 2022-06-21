﻿using LagerPlayground.Data;
using LagerPlayground.Models;
using LagerPlayground.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class PickingController : Controller
    {
        private readonly Context _context;

        public PickingController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            OrdersReadyToPickCount vmOrders = new();
            vmOrders.AllOrdersNumber = _context.Order_Details.ToList().Count;

            return View(vmOrders);
        }

        public IActionResult MultiPickSite()
        {
            return View();
        }

        public async Task<JsonResult> GetOrdersToPick(int numberOfOrders)
        {
            if (numberOfOrders == 0)
            {
                return Json(new { booleanError = true, msg = "Batch Amount can't be 0" });
            }

            var orders = await _context.Order_Details
                .Take(numberOfOrders)
                .Where(x => x.OrderStatus != "Complete" || x.OrderStatus != "Picking")
                .Include(x => x.Order_Items)
                .ThenInclude(x => x.Product)
                .AsNoTracking().ToListAsync();

            if (orders.Count == 0)
            {
                return Json(new { booleanError = true, msg = "No orders was found" });
            }

            var productLocations = await _context.Product_Locations
                .Where(x => x.LocationBarcode != "Receiving-Station" && x.Locations_Positions.Pickable != false)
                .AsNoTracking().ToListAsync();

            if(productLocations.Count == 0)
            {
                return Json(new { booleanError = true, msg = "No product locations was found" });
            }

            int itemsToPick = 0;
            int ordersToPick = 0;

            List<DTOPickLocation> dtoPickLocations = new();
            foreach (var order in orders)
            {
                ordersToPick++;
                foreach (var item in order.Order_Items)
                {
                    itemsToPick += item.Quantity;
                    int newOrderQuantity = item.Quantity;
                    for (int i = 0; i < productLocations.Count; i++)
                    {
                        bool stopLoop = false;
                        if (item.ProductID == productLocations[i].ProductID)
                        {
                            DTOPickLocation dtoPickLocation = new();
                            dtoPickLocation.Order_DetailsID = item.Order_DetailsID;
                            dtoPickLocation.ProductID = item.ProductID;
                            dtoPickLocation.ProductImage = item.Product.Image;
                            dtoPickLocation.ProductName = item.Product.Name;
                            dtoPickLocation.ProductBarcode = item.Product.BarcodeID;
                            dtoPickLocation.LocationBarcode = productLocations[i].LocationBarcode;
                            dtoPickLocation.OnHandQuantity = productLocations[i].Quantity;
                            dtoPickLocation.OrderStatus = order.OrderStatus;
                            dtoPickLocation.PickingToteBarcode = item.PickingToteBarcode;

                            int productQuantity = 0;
                            int subResult = 0;

                            if (productLocations[i].Quantity != 0 && newOrderQuantity != 0)
                            {
                                // If order item quantity is less than the quantity lays in a product location, it will say you should pick from tahat location.
                                if (newOrderQuantity <= productLocations[i].Quantity)
                                {
                                    subResult = productLocations[i].Quantity - newOrderQuantity;
                                    productQuantity = newOrderQuantity;
                                    newOrderQuantity -= subResult;

                                    productLocations[i].Quantity = subResult;
                                    stopLoop = true;
                                }
                                else // If not, it will add the next location, you would need to go to, to get the rest of the same product.
                                {
                                    subResult = newOrderQuantity - productLocations[i].Quantity;
                                    productQuantity = newOrderQuantity - subResult;

                                    if (newOrderQuantity < 1)
                                    {
                                        stopLoop = true;
                                    }

                                    newOrderQuantity = subResult;
                                    productLocations[i].Quantity = 0;
                                }

                                dtoPickLocation.PickQuantity = productQuantity;
                                dtoPickLocations.Add(dtoPickLocation);

                                if (stopLoop)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            var sortedPickLocations = dtoPickLocations.OrderBy(x => x.LocationBarcode).ToList();
            
            List<DTOPickLocation> mergedLocations = new();
            foreach (var sortedPickLocation in sortedPickLocations)
            {
                //-Until i find a better solution
                DTOPickLocation newdtoPickLocation = new()
                {
                    Order_DetailsID = 0,
                    ProductID = sortedPickLocation.ProductID,
                    LocationBarcode = sortedPickLocation.LocationBarcode,
                    ProductBarcode = sortedPickLocation.ProductBarcode,
                    ProductName = sortedPickLocation.ProductName,
                    ProductImage = sortedPickLocation.ProductImage,
                    PickQuantity = sortedPickLocation.PickQuantity,
                    OnHandQuantity = sortedPickLocation.OnHandQuantity,
                    OrderStatus = null,
                    PickingToteBarcode = null
                };
                //

                int? containsThis = null;

                if (mergedLocations.Count != 0)
                {
                    for (var i = 0; i < mergedLocations.Count; i++)
                    {
                        if (newdtoPickLocation.ProductID == mergedLocations[i].ProductID && newdtoPickLocation.LocationBarcode == mergedLocations[i].LocationBarcode)
                        {
                            containsThis = i;
                            break;
                        }
                    }

                    if (containsThis != null)
                    {
                        mergedLocations[(int)containsThis].PickQuantity += newdtoPickLocation.PickQuantity;
                    }
                    else
                    {
                        mergedLocations.Add(newdtoPickLocation);
                    }
                }
                else
                {
                    mergedLocations.Add(newdtoPickLocation);
                }
            }

            return Json(new { booleanError = false, msg = orders.Count + " orders to pick", sortedPickLocations, mergedLocations, itemsToPick, ordersToPick });
        }

        public async Task<JsonResult> GetAllTotes()
        {
            var totes = await _context.Totes
                .Where(x => x.InUse == false)
                .AsNoTracking().ToListAsync();

            if (totes.Count == 0)
            {
                return Json(new { booleanError = true, msg = "No totes where found" });
            }

            return Json(new { booleanError = false, totes });

        }

        public async Task<JsonResult> GetTote(string scannedBarcode)
        {

            if (scannedBarcode == null || scannedBarcode.Trim() == "")
            {
                return Json(new { booleanError = true, msg = "No barcode was scanned, try again" });
            }

            var tote = await _context.Totes
                .AsNoTracking().FirstOrDefaultAsync(x => x.Barcode == scannedBarcode);

            if (tote == null)
            {
                return Json(new { booleanError = true, msg = "No tote white the scanned barcode was found, try again" });
            }

            return Json(new { booleanError = false, tote });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddToTote(int? orderID, int? productID, int? pickedQuantity, int? onHandQuantity, string toteBarcode, string locationBarcode)
        {
            if (orderID == 0 || orderID == null)
            {
                return Json(new { booleanError = true, msg = "No Order ID was found" });
            }

            if (productID == 0 || productID == null)
            {
                return Json(new { booleanError = true, msg = "No Product ID was found" });
            }

            if (pickedQuantity == 0 || pickedQuantity == null)
            {
                return Json(new { booleanError = true, msg = "Picked quantity can't be 0 or NULL" });
            }

            if (onHandQuantity == 0 || onHandQuantity == null)
            {
                return Json(new { booleanError = true, msg = "No On Hand Quantity was found" });
            }

            if (locationBarcode == null)
            {
                return Json(new { booleanError = true, msg = "No Location Barcode was found" });
            }

            var orderDetails = await _context.Order_Details
               .FindAsync(orderID);

            if (orderDetails == null)
            {
                return Json(new { booleanError = true, msg = "No Order was found" });
            }

            var orderItem = await _context.Order_Items
                .Include(x =>  x.Product)
                .FirstOrDefaultAsync(x => x.Order_DetailsID == orderID && x.ProductID == productID);

            if (orderItem == null)
            {
                return Json(new { booleanError = true, msg = "No Order Item was found" });
            }

            var tote = await _context.Totes
                .FirstOrDefaultAsync(x => x.Barcode == toteBarcode);

            if (tote == null)
            {
                return Json(new { booleanError = true, msg = "No Tote with the scanned barcode was found" });
            }

            if (tote.Order_DetailsID != null && tote.Order_DetailsID != orderID)
            {
                return Json(new { booleanError = true, msg = "The selected Tote is already in use by another order" });
            }

            int newPickedQuantity = orderItem.PickingQuantity += (int)pickedQuantity;
            string newOrderStatus = orderDetails.OrderStatus;

            try
            {
                if (orderDetails.OrderStatus == "Pending")
                {
                    if (await TryUpdateModelAsync<Order_Details>(
                        orderDetails,
                        "",
                         x => x.OrderStatus, x => x.Modified))
                    {
                        orderDetails.OrderStatus = "Picking";
                        orderDetails.Modified = DateTime.Now;
                    }
                    newOrderStatus = "Picking";
                    _context.Order_Details.Update(orderDetails);
                }

                if (newPickedQuantity <= orderItem.Quantity)
                {
                    if (await TryUpdateModelAsync<Order_Items>(
                        orderItem,
                        "",
                        x => x.PickingQuantity, x => x.Modified, x => x.PickingToteBarcode))
                    {
                        orderItem.PickingQuantity = newPickedQuantity;
                        orderItem.PickingToteBarcode = toteBarcode;
                        orderItem.Modified = DateTime.Now;
                    }

                    _context.Order_Items.Update(orderItem);
                }

                if (tote.Order_DetailsID == null)
                {
                    if (await TryUpdateModelAsync<Tote>(
                        tote,
                        "",
                        x => x.Order_DetailsID, x => x.Modified))
                    {
                        tote.Order_DetailsID = orderID;
                        tote.Modified = DateTime.Now;
                    }
                    _context.Totes.Update(tote);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, msg = "An database error has occured" });
            }

            // FIX
            int newPickQuantity = orderItem.Quantity -= (int)pickedQuantity;
            DTOPickLocation dtoPickLocation = new()
            {
                Order_DetailsID = orderDetails.ID,
                ProductID = orderItem.ProductID,
                ProductImage = orderItem.Product.Image,
                ProductName = orderItem.Product.Name,
                ProductBarcode = orderItem.Product.BarcodeID,
                PickQuantity = newPickQuantity,
                OnHandQuantity = (int)(onHandQuantity -= newPickQuantity),
                LocationBarcode = locationBarcode,
                OrderStatus = newOrderStatus,
                PickingToteBarcode = toteBarcode
            };

            return Json(new { booleanError = false, dtoPickLocation, newPickQuantity });
        }
    }
}