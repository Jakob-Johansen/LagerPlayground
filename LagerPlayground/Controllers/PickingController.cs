using LagerPlayground.Data;
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
            int containsThis = 0;
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
                    OnHandQuantity = sortedPickLocation.OnHandQuantity
                };
                //

                containsThis = 0;

                if (mergedLocations.Count != 0 || mergedLocations != null)
                {
                    for (var i = 0; i < mergedLocations.Count; i++)
                    {
                        if (newdtoPickLocation.ProductID == mergedLocations[i].ProductID && newdtoPickLocation.LocationBarcode == mergedLocations[i].LocationBarcode)
                        {
                            containsThis = i;
                        }
                    }

                    if (containsThis != 0)
                    {
                        mergedLocations[containsThis].PickQuantity += newdtoPickLocation.PickQuantity;
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

        public async Task<JsonResult> GetTotes(int orderID)
        {
            if (orderID == 0)
            {
                return Json(new { booleanError = true, msg = "No orderID was received" });
            }

            var totes = await _context.Totes
                .Where(x => x.InUse == false)
                .AsNoTracking().ToListAsync();

            if (totes.Count == 0)
            {
                return Json(new { booleanError = true, msg = "No totes where found" });
            }

            return Json(new { booleanError = false, totes });

        }
    }
}