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

        // Når man creater en location skal man kunne custom tilføje flere racks og shelfs.
        // Fx. for hver rack skal man kunne customize shelfs og bins.
        // Gør så man ikke behøver at lave bins hvis man ikke vil det.

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
        public async Task<IActionResult> CreateLocation([Bind("Warehouse,Section,Row")] Locations locations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    locations.Created = DateTime.Now;
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

        public async Task<IActionResult> LocationDetails(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return NotFound();
            }

            var location = await _context.Locations.Include(x => x.Locations_Details).FirstOrDefaultAsync(x => x.ID == ID);
            return View(location);
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

        public async Task<IActionResult> CreateRack(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var getRack = await _context.Locations_Details.OrderByDescending(x => x.Rack).FirstOrDefaultAsync(x => x.LocationsID == ID);

            int rackNumber = 0;

            if (getRack == null)
            {
                rackNumber = 1;
            }
            else
            {
                rackNumber = getRack.Rack + 1;
            }

            ViewBag.RackNumber = rackNumber;
            ViewBag.LocationID = ID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRack([Bind("Rack,Shelfs,Bins,LocationsID")] Locations_details locations_Details)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    locations_Details.Created = DateTime.Now;
                    _context.Locations_Details.Add(locations_Details);
                    await _context.SaveChangesAsync();
                    return Redirect("/Location/LocationDetails/" + locations_Details.LocationsID);
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }
            return View(locations_Details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRack(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var removeThis = await _context.Locations_Details.FindAsync(ID);
            int locationID = removeThis.LocationsID;

            try
            {
                _context.Locations_Details.Remove(removeThis);
                await _context.SaveChangesAsync();
                return Redirect("/Location/LocationDetails/" + locationID);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }
            return NotFound();
        }

        //public async Task<JsonResult> LocationDeleteOne(int? ID) 
        //{
        //    if (ID == null)
        //    {
        //        return Json(new { errorBoolean = true, errorMsg = "No ID was found" });
        //    }

        //    var Location = await _context.Locations.FindAsync(ID);

        //    if (Location == null)
        //    {
        //        return Json(new { errorBoolean = true, errorMsg = "No location was found" });
        //    }

        //    try
        //    {
        //        _context.Locations.Remove(Location);
        //        await _context.SaveChangesAsync();
        //        return Json(new { errorBoolean = false });
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return Json(new { errorBoolean = true, errorMsg = "An error with the database has occured" });
        //    }
        //}
    }
}
