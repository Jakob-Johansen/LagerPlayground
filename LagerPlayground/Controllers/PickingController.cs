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

            List<DTOPickLocation> dtoPickLocations = new();
            foreach (var order in orders)
            {
                foreach (var item in order.Order_Items)
                {
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

            var sortedPickLocations = dtoPickLocations.OrderBy(x => x.LocationBarcode);
            return Json(new { booleanError = false, msg = orders.Count + " orders to pick", sortedPickLocations });
        }
    }
}