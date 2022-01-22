using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LagerPlayground.Helpers;
using LagerPlayground.Models.VM;
using LagerPlayground.Models;

namespace LagerPlayground.Controllers
{
    public class ShopController : Controller
    {
        // https://stackoverflow.com/questions/46921275/access-session-variable-in-razor-view-net-core-2
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
        // https://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/

        public readonly Context _context;

        public ShopController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Get("cart") != null)
            {
                int sessionCount = 0;
                foreach (var sessionProduct in SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart"))
                {
                    sessionCount += sessionProduct.Quantity;
                }

                ViewBag.SessionCount = sessionCount;
            }

            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            return View();
        }

        public IActionResult CheckOut()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut([Bind("Name,Email,PhoneNumber,MobileNumber,Country,City,Zipcode,Address")] Custommer custommer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    custommer.Created = DateTime.Now;
                    _context.Add(custommer);
                    await _context.SaveChangesAsync();

                    // https://entityframework.net/retrieve-id-of-inserted-entity
                    // Får id fra den custommer som lige er blevet gemt.
                    //ViewBag.id = custommer.ID;

                    Order_Details order_Details = new();
                    order_Details.CustommerID = custommer.ID;
                    order_Details.Created = DateTime.Now;
                    _context.Add(order_Details);
                    await _context.SaveChangesAsync();

                    List<Order_Items> order_ItemsList = new();

                    foreach (var item in SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart"))
                    {
                        Order_Items order_Items = new();
                        order_Items.Order_DetailsID = order_Details.ID;
                        order_Items.ProductID = item.Product.ID;
                        order_Items.Quantity = item.Quantity;
                        order_Items.Created = DateTime.Now;
                        order_ItemsList.Add(order_Items);
                    }

                    await _context.AddRangeAsync(order_ItemsList);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove("cart");

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }

            return View(custommer);
        }

        public async Task Buy(int id, int? quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new();
                cart.Add(new Item { Product = product, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = IsExisting(id);
                if (index != -1)
                {
                    if (quantity != null)
                    {
                        cart[index].Quantity = 0;
                        cart[index].Quantity = (int)quantity;
                    }
                    else
                    {
                        cart[index].Quantity++;
                    }
                }
                else
                {
                    cart.Add(new Item { Product = product, Quantity = 1});
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
        }

        public void Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = IsExisting(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        }

        private int IsExisting(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ID.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
