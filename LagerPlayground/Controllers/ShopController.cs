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

        public async Task ShopBuy(int id, int? quantity)
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
                int index = IsExisting(id, "cart");
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

        public void ShopRemove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = IsExisting(id, "cart");
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        }

        private int IsExisting(int id, string sessionName)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, sessionName);
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ID.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }

        //---Receive Shop---

        public async Task<IActionResult> ReceiveShop()
        {
            if (HttpContext.Session.Get("ReceiveCart") != null)
            {
                int sessionCount = 0;
                foreach (var sessionProduct in SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart"))
                {
                    sessionCount += sessionProduct.Quantity;
                }

                ViewBag.SessionCount = sessionCount;
            }

            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult ReceiveCart()
        {
            var receiveCart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart");
            ViewBag.receiveCart = receiveCart;
            return View();
        }

        public IActionResult ReceiveCheckOut()
        {
            var receiveCart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart");
            ViewBag.receiveCart = receiveCart;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceiveCheckOut([Bind("CompanyName")] ReceiveCustommer receiveCustommer, [Bind("ArrivalDate")] ReceivingOrder_Details receivingOrder_Details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    receiveCustommer.Created = DateTime.Now;
                    _context.ReceiveCustommers.Add(receiveCustommer);
                    await _context.SaveChangesAsync();

                    // https://entityframework.net/retrieve-id-of-inserted-entity
                    // Får id fra den custommer som lige er blevet gemt.
                    //ViewBag.id = custommer.ID;

                    receivingOrder_Details.ReceiveCustommerID = receiveCustommer.ID;
                    receivingOrder_Details.Created = DateTime.Now;
                    _context.ReceivingOrder_Details.Add(receivingOrder_Details);
                    await _context.SaveChangesAsync();

                    List<ReceivingOrder_Items> receivingOrder_ItemsList = new();

                    foreach (var item in SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart"))
                    {
                        ReceivingOrder_Items receivingOrder_Items = new();
                        receivingOrder_Items.ReceivingOrder_DetailsID = receivingOrder_Details.ID;
                        receivingOrder_Items.ProductID = item.Product.ID;
                        receivingOrder_Items.Quantity = item.Quantity;
                        receivingOrder_Items.Created = DateTime.Now;
                        receivingOrder_ItemsList.Add(receivingOrder_Items);
                    }

                    await _context.AddRangeAsync(receivingOrder_ItemsList);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove("ReceiveCart");

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }

            return View(receiveCustommer);
        }

        public async Task ReceiveBuy(int id, int? quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart") == null)
            {
                List<Item> cart = new();
                cart.Add(new Item { Product = product, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ReceiveCart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart");
                int index = IsExisting(id, "ReceiveCart");
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
                    cart.Add(new Item { Product = product, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ReceiveCart", cart);
            }
        }

        public void ReceiveRemove(int id)
        {
            List<Item> receiveCart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "ReceiveCart");
            int index = IsExisting(id, "ReceiveCart");
            receiveCart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "ReceiveCart", receiveCart);
        }
    }
}
