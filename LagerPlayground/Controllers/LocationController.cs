using LagerPlayground.Data;
using LagerPlayground.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class LocationController : Controller
    {
        private readonly Context _context;
        public LocationController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Locations()
        {
            var allLocations = await _context.Locations.ToListAsync();
            return View(allLocations);
        }

        public IActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation([Bind("Warehouse,Section,Row,Rack,Shelf,Bin")] Locations locations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    locations.Dynamic = false;
                    _context.Locations.Add(locations);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Locations");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }
            return View(locations);
        }

        public async Task<JsonResult> LocationDeleteOne(int? ID) 
        {
            if (ID == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "No ID was found" });
            }

            var Location = await _context.Locations.FindAsync(ID);

            if (Location == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "No location was found" });
            }

            try
            {
                _context.Locations.Remove(Location);
                await _context.SaveChangesAsync();
                return Json(new { errorBoolean = false });
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LocationDeleteSelected(List<int?> IDs)
        {
            if (IDs == null)
            {
                return Json(new { errorBoolean = true, errorMsg = "No Location was selected" });
            }

            List<Locations> locationsToDeleteList = new();
            foreach (int? ID in IDs)
            {
                var location = await _context.Locations.FindAsync(ID);
                if (location != null)
                {
                    locationsToDeleteList.Add(location);
                }
            }

            try
            {
                _context.Locations.RemoveRange(locationsToDeleteList);
                await _context.SaveChangesAsync();
                return Json(new { errorBoolean = false });
            }
            catch (DbUpdateException)
            {
                return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
            }
        }
    }
}
