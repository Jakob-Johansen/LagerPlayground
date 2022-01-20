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
        public readonly Context _context;

        public ShopController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            return View();
        }

        public async Task<IActionResult> Buy(int id)
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
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = product, Quantity = 1});
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
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
