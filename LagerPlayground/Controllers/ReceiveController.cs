using LagerPlayground.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class ReceiveController : Controller
    {
        private readonly Context _context;
        public ReceiveController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Receive(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var receiveOrderDetails = await _context.ReceivingOrder_Details
                .Include(x => x.ReceivingOrder_Items)
                    .ThenInclude(t => t.Product)
                .FirstOrDefaultAsync();

            if (receiveOrderDetails == null)
            {
                return NotFound();
            }

            return View(receiveOrderDetails);
        }
    }
}
