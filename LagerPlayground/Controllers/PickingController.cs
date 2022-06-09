using LagerPlayground.Data;
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

            //var orders = await _context.Order_Details
            //    .Take(numberOfOrders)
            //    .Include(x => x.Order_Items)
            //    .AsNoTracking().ToListAsync();

            var oneOrder = await _context.Order_Details
                .Include(x => x.Order_Items)
                .ThenInclude(x => x.Product)
                .AsNoTracking().FirstOrDefaultAsync();

            var productLocations = await _context.Product_Locations
                .OrderByDescending(x => x.LocationBarcode)
                .Where(x => x.LocationBarcode != "Receiving-Station")
                .AsNoTracking().ToListAsync();

            List<DTOTest> dtoTests = new();
            foreach (var item in oneOrder.Order_Items)
            {
                int orderQuantity = item.Quantity;
                DTOTest dtoTest = new();
                dtoTest.Order_DetailsID = item.Order_DetailsID;
                dtoTest.ProductID = item.ProductID;
                dtoTest.ProductName = item.Product.Name;
                dtoTest.ProductBarcode = item.Product.BarcodeID;

                foreach (var pLocation in productLocations)
                {
                    if(item.ProductID == pLocation.ProductID)
                    {
                        // VIRKERR IKKE!
                        if (orderQuantity >= pLocation.Quantity)
                        {
                            orderQuantity -= pLocation.Quantity;
                            int myTest = item.Quantity - orderQuantity;

                            dtoTest.ProductQuantity = 10;
                            dtoTest.LocationBarcode = pLocation.LocationBarcode;

                            dtoTests.Add(dtoTest);
                            if (orderQuantity != 0)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            dtoTest.ProductQuantity = 20;
                            dtoTest.LocationBarcode = pLocation.LocationBarcode;
                            dtoTests.Add(dtoTest);
                            break;
                        }
                    }
                }
            }

            return Json(new { booleanError = false, msg = 1 + " orders to pick", dtoTests });
            //return Json(new { booleanError = false, msg = orders.Count + " orders to pick", orders });
        }
    }
}
