using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class ScannerController : Controller
    {
        private readonly Context _context;
        public ScannerController(Context context)
        {
            _context = context;
        }

        // Scanner Fun
        public async Task<JsonResult> GetJson(string productID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productID);

            if (product == null)
            {
                return Json(false);
            }

            return Json(product);
        }

        // Arrivals

    }
}
