using LagerPlayground.Data;
using LagerPlayground.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class PackingController : Controller
    {
        private readonly Context _context;

        public PackingController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Order_Details
                .Where(x => x.OrderStatus == "Picking")
                .Include(x => x.Order_Items.Take(3))
                    .ThenInclude(x => x.Product)
                .AsNoTracking().ToListAsync();

            List<DTOPack> dtoPackList = new();

            foreach (var order in orders)
            {
                int items = 0;
                List<string> productImages = new();
                foreach (var item in order.Order_Items)
                {
                    items += item.Quantity;

                    productImages.Add(item.Product.Image);
                }

                var dtoPack = new DTOPack
                {
                    Id = order.ID,
                    Tote = order.Order_Items.ToArray()[0].PickingToteBarcode,
                    Items = items,
                    ProductImages = productImages,
                    Status = order.OrderStatus,
                    ShipByDate = order.Created
                };

                dtoPackList.Add(dtoPack);
            }

            return View(dtoPackList);
        }

        public async Task<IActionResult> Pack(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var order = await _context.Order_Details
                .Include(x => x.Custommer)
                .Include(x => x.Order_Items)
                    .ThenInclude(x => x.Product)
                .AsNoTracking().FirstOrDefaultAsync(x => x.ID == Id);

            if (order == null)
            {
                return NotFound();
            }

            int items = 0;

            foreach (var item in order.Order_Items)
            {
                items += item.PickingQuantity;
            }

            ViewBag.items = items;
            ViewBag.tote = order.Order_Items.ToArray()[0].PickingToteBarcode;

            return View(order);
        }
    }
}
