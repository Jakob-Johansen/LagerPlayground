using LagerPlayground.Data;
using LagerPlayground.Helpers;
using LagerPlayground.Models;
using LagerPlayground.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerPlayground.Controllers
{
    public class LocationController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;
        public LocationController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Når man creater en location skal man kunne custom tilføje flere racks og shelfs.
        // Fx. for hver rack skal man kunne customize shelfs og bins.
        // Gør så man ikke behøver at lave bins hvis man ikke vil det.

        // https://docs.linnworks.com/articles/#!documentation/wms-location/a/WMSenablelocation

        // Bare til tests
        public async Task<IActionResult> AllLocations()
        {
            var AllLocations = await _context.Locations
                .Include(x => x.locations_Racks)
                    .ThenInclude(t => t.Locations_Shelfs)
                        .ThenInclude(y => y.Locations_Positions)
                .AsNoTracking().ToListAsync();

            return View(AllLocations);
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

            var location = await _context.Locations.Include(x => x.locations_Racks).FirstOrDefaultAsync(x => x.ID == ID);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRack(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            Locations_Racks locations_Racks = new();

            var getRack = await _context.Locations_Racks.OrderByDescending(x => x.RackNumber).FirstOrDefaultAsync(x => x.LocationsID == ID);

            int rackNumber = 0;

            if (getRack == null)
            {
                rackNumber = 1;
            }
            else
            {
                rackNumber = getRack.RackNumber + 1;
            }

            try
            {
                if (ModelState.IsValid)
                {
                    locations_Racks.Created = DateTime.Now;
                    locations_Racks.RackNumber = rackNumber;
                    locations_Racks.LocationsID = (int)ID;
                    _context.Locations_Racks.Add(locations_Racks);
                    await _context.SaveChangesAsync();
                    return Redirect("/Location/LocationDetails/" + ID);
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRack(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var removeThis = await _context.Locations_Racks.FindAsync(ID);
            int locationID = removeThis.LocationsID;

            try
            {
                _context.Locations_Racks.Remove(removeThis);
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

        public async Task<IActionResult> RackDetails(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var locationRack = await _context.Locations_Racks
                    .Include(x => x.Locations)
                    .Include(x => x.Locations_Shelfs)
                        .ThenInclude(t => t.Locations_Positions)
                    .FirstOrDefaultAsync(x => x.ID == ID);

            if (locationRack == null)
            {
                return NotFound();
            }

            return View(locationRack);
        }

        // New Version
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateShelfAndBins(int? binQuantity, int? shelfQuantity, int? rackID, int rackNumber, string row)
        {
            if (rackID == null || rackID == 0)
            {
                return Json(new { booleanError = false, msg = "No RackID was found" });
            }

            if (binQuantity == null || binQuantity < 1 && shelfQuantity == null || shelfQuantity < 1)
            {
                return Json(new { booleanError = false, msg = "Bin and Shelf input field need to be more than 0" });
            }

            if (binQuantity == null || binQuantity < 1)
            {
                return Json(new { booleanError = false, msg = "Bin input field need to be more than 0" });
            }

            if (shelfQuantity == null || shelfQuantity < 1)
            {
                return Json(new { booleanError = false, msg = "Shelf input field need to be more than 0" });
            }
            try
            {
                var locationShelf = await _context.Locations_Shelfs.OrderByDescending(x => x.ShelfNumber)
                    .FirstOrDefaultAsync(x => x.Locations_RacksID == rackID);

                int locationShelfNumber = 0;
                if (locationShelf == null)
                {
                    locationShelfNumber = 1;
                }
                else
                {
                    locationShelfNumber = locationShelf.ShelfNumber + 1;
                }

                List<Locations_Shelfs> locations_ShelfsList = new();
                for (int i = 1; i < shelfQuantity + 1; i++)
                {
                    Locations_Shelfs locations_Shelfs = new();
                    locations_Shelfs.Locations_RacksID = (int)rackID;
                    locations_Shelfs.ShelfNumber = locationShelfNumber;
                    locations_Shelfs.Created = DateTime.Now;

                    locations_ShelfsList.Add(locations_Shelfs);
                    locationShelfNumber++;
                }

                _context.Locations_Shelfs.AddRange(locations_ShelfsList);
                await _context.SaveChangesAsync();

                List<Locations_Positions> locations_PositionsList = new();

                foreach (var locationShelfsList in locations_ShelfsList)
                {
                    for (int i = 1; i < binQuantity + 1; i++)
                    {
                        Locations_Positions locations_Positions = new();
                        locations_Positions.Locations_ShelfsID = locationShelfsList.ID;
                        locations_Positions.PositionNumber = i;
                        locations_Positions.Created = DateTime.Now;
                        //locations_Positions.Pickable = true;
                        locations_Positions.FullLocationBarcode = row + (rackNumber < 10 ? "-0" + rackNumber : "-" + rackNumber) + (locationShelfsList.ShelfNumber < 10 ? "-0" + locationShelfsList.ShelfNumber : "-" + locationShelfsList.ShelfNumber) + (i < 10 ? "-0" + i : "-" + i);
                        locations_PositionsList.Add(locations_Positions);
                    }
                }

                _context.AddRange(locations_PositionsList);
                await _context.SaveChangesAsync();

                List<VMShelfAndPositions> vmShelfAndPositionsList = new();
                foreach (var locationShelfList in locations_ShelfsList)
                {
                    VMShelfAndPositions vmShelfAndPositions = new();
                    vmShelfAndPositions.ShelfID = locationShelfList.ID;
                    vmShelfAndPositions.ShelfNumber = locationShelfList.ShelfNumber;
                    vmShelfAndPositions.PositionLength = locations_PositionsList.Where(x => x.Locations_ShelfsID == locationShelfList.ID).Count();
                    vmShelfAndPositions.VMPositions = new();

                    foreach (var locationPositionList in locations_PositionsList.OrderByDescending(x => x.PositionNumber).Where(x => x.Locations_ShelfsID == locationShelfList.ID))
                    {
                        VMPositions vmPositions = new();
                        vmPositions.PositionID = locationPositionList.ID;
                        vmPositions.PositionNumber = locationPositionList.PositionNumber;

                        vmShelfAndPositions.VMPositions.Add(vmPositions);
                    }
                    vmShelfAndPositionsList.Add(vmShelfAndPositions);
                }

                return Json(new { booleanError = true, locationData = vmShelfAndPositionsList });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = false, msg = "An Database error has occured" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PrintLocationBarcodes(List<int> IDs)
        {
            if (IDs == null)
            {
                return Json(new { booleanError = true, errorMsg = "No IDs was found"});
            }

            List<PdfModel> pdfModels = new();

            IDs.Sort();
            foreach (var item in IDs)
            {
                var locationPosition = await _context.Locations_Positions.FirstOrDefaultAsync(x => x.ID == item);
                if (locationPosition != null)
                {
                    pdfModels.Add(new PdfModel
                    {
                        Barcode = locationPosition.FullLocationBarcode,
                        Name = locationPosition.FullLocationBarcode
                    });
                }
            }

            PdfHelper pdfHelper = new(_env);
            pdfHelper.GenerateBarcodesPrint(pdfModels);

            return Json(new { booleanError = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddBinToShelf(int? ShelfID)
        {
            if (ShelfID == null)
            {
                return Json(new { booleanError = true, errorMsg = "No ide was found" });
            }

            var shelf = await _context.Locations_Shelfs
                .Include(x => x.Locations_Positions)
                .Include(x => x.Locations_Rack)
                    .ThenInclude(t => t.Locations)
                .FirstOrDefaultAsync(x => x.ID == ShelfID);

            if (shelf == null)
            {
                return Json(new { booleanError = true, errorMsg = "No shelf was found" });
            }

            var getNewPositionNumber = 0;

            if (!shelf.Locations_Positions.Any())
            {
                getNewPositionNumber = 1;
            }
            else
            {
                getNewPositionNumber = shelf.Locations_Positions.ToArray()[shelf.Locations_Positions.Count() - 1].PositionNumber + 1;
            }
            
            try
            {
                Locations_Positions locations_Positions = new();
                locations_Positions.Locations_ShelfsID = (int)ShelfID;
                locations_Positions.Created = DateTime.Now;
                locations_Positions.PositionNumber = getNewPositionNumber;
                locations_Positions.FullLocationBarcode = shelf.Locations_Rack.Locations.Row + (shelf.Locations_Rack.RackNumber < 10 ? "-0" + shelf.Locations_Rack.RackNumber : "-" + shelf.Locations_Rack.RackNumber) + (shelf.ShelfNumber < 10 ? "-0" + shelf.ShelfNumber : "-" + shelf.ShelfNumber) + (getNewPositionNumber < 10 ? "-0" + getNewPositionNumber : "-" + getNewPositionNumber);

                _context.Locations_Positions.Add(locations_Positions);
                await _context.SaveChangesAsync();

                return Json(new { booleanError = false, positionID = locations_Positions.ID, positionNumber = getNewPositionNumber });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, errorMsg = "An database error has occured" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteBinFromShelf(int? ShelfID)
        {
            if (ShelfID == null)
            {
                return Json(new { booleanError = true, errorMsg = "No shelf Id was found" });
            }

            var position = await _context.Locations_Positions
                .OrderByDescending(x => x.PositionNumber)
                .FirstOrDefaultAsync(x => x.Locations_ShelfsID == ShelfID);

            if (position == null)
            {
                return Json(new { booleanError = false, noPositions = true});
            }

            int positionNumber = position.PositionNumber;

            try
            {
                _context.Locations_Positions.Remove(position);
                await _context.SaveChangesAsync();

                positionNumber--;

                return Json(new { booleanError = false, positionNumber });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, errorMsg = "An database error has occured" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTopShelf(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var shelf = await _context.Locations_Shelfs
                .OrderByDescending(x => x.ShelfNumber)
                .FirstOrDefaultAsync(x => x.Locations_RacksID == ID);

            if (shelf == null)
            {
                return Redirect("/Location/RackDetails/" + ID);
            }

            //var getShelf = 0;
            //if (!rack.Locations_Shelfs.Any())
            //{
            //    return Redirect("/Location/RackDetails/" + ID);
            //}
            //else
            //{
            //    getShelf = rack.Locations_Shelfs.ToArray()[rack.Locations_Shelfs.Count() - 1].ID;
            //}

            try
            {
                _context.Locations_Shelfs.Remove(shelf);
                await _context.SaveChangesAsync();
                return Redirect("/Location/RackDetails/" + ID);
            }
            catch (DbUpdateException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> MakePickableOrNonPickable(List<int> IDList, bool makePickable)
        {
            if (IDList.Count == 0)
            {
                string msg;
                if (makePickable == true)
                    msg = "No positions was selected or the positions is already pickable";
                else
                    msg = "No positions was selected or the positions is already non pickable";

                return Json(new { booleanError = true, errorMsg = msg });
            }

            List<Locations_Positions> locations_PositionsList = new();
            foreach (var item in IDList)
            {
                var positionToUpdate = await _context.Locations_Positions.FirstOrDefaultAsync(x => x.ID == item);

                if (positionToUpdate == null)
                {
                    return Json(new { booleanError = true, errorMsg = "One or more positions could not be found" });
                }

                if (await TryUpdateModelAsync<Locations_Positions>(
                    positionToUpdate,
                    "",
                    x => x.Pickable, x => x.Modified))
                {
                    if (makePickable == true)
                    {
                        positionToUpdate.Pickable = true;
                    }
                    else
                    {
                        positionToUpdate.Pickable = false;
                    }

                    positionToUpdate.Modified = DateTime.Now;
                    locations_PositionsList.Add(positionToUpdate);
                }
            }

            try
            {
                _context.Locations_Positions.UpdateRange(locations_PositionsList);
                await _context.SaveChangesAsync();
                return Json(new { booleanError = false });
            }
            catch (DbUpdateException)
            {
                return Json(new { booleanError = true, errorMsg = "An database error has occured" });
            }
        }
    }
}
