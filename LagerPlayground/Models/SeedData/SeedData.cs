using LagerPlayground.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models.SeedData
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new Context(serviceProvider.GetRequiredService<DbContextOptions<Context>>());

            if (context.Products.Any())
            {
                return;
            }

            context.Products.AddRange(
                new Product
                {
                    Name = "WIRELESS BARCODE SCANNER",
                    Description = "En stregkode scanner som kan scanne stregkoder",
                    BrandName = "NETUM",
                    Category = "Elektronik",
                    BarcodeID = "K50674",
                    Image = "K50675.png",
                    Quantity = 1000
                },

                new Product
                {
                    Name = "ModMic USB",
                    Description = "En Mikrofon",
                    BrandName = "ANTLION AUDIO",
                    Category = "Elektronik",
                    BarcodeID = "761878978616",
                    Image = "2804253_5ebbb392ba6f.jpg",
                    Quantity = 1000
                },

                new Product
                {
                    Name = "ZINZINO XTEND+",
                    Description = "Vitaminer",
                    BrandName = "ZINZINO",
                    Category = "Fødevare",
                    BarcodeID = "302205",
                    Image = "zinzino-xtend-natural-essential-vitamins-minerals2.png",
                    Quantity = 1000
                }
            );

            //context.Locations.Add(
            //    new Locations
            //    {
            //        Section = "Hal 1",
            //        Warehouse = "Primary",
            //        Row = "A",
            //        Dynamic = true,
            //        Created = DateTime.Now,
            //    }
            //);

            //context.Locations_Racks.Add(
            //    new Locations_Racks
            //    {
            //        LocationsID = 1,
            //        RackNumber = 1,
            //        Created = DateTime.Now
            //    }
            //);

            //context.Locations_Shelfs.AddRange(
            //    new Locations_Shelfs
            //    {
            //        Locations_RacksID = 1,
            //        ShelfNumber = 1,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Shelfs
            //    {
            //        Locations_RacksID = 1,
            //        ShelfNumber = 2,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Shelfs
            //    {
            //        Locations_RacksID = 1,
            //        ShelfNumber = 3,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Shelfs
            //    {
            //        Locations_RacksID = 1,
            //        ShelfNumber = 4,
            //        Created = DateTime.Now
            //    }
            //);

            //context.Locations_Positions.AddRange(
            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 1,
            //        PositionNumber = 1,
            //        FullLocationBarcode = "A-01-01-01",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 1,
            //        PositionNumber = 2,
            //        FullLocationBarcode = "A-01-01-02",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 1,
            //        PositionNumber = 3,
            //        FullLocationBarcode = "A-01-01-03",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 1,
            //        PositionNumber = 4,
            //        FullLocationBarcode = "A-01-01-04",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 1,
            //        PositionNumber = 5,
            //        FullLocationBarcode = "A-01-01-05",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 2,
            //        PositionNumber = 1,
            //        FullLocationBarcode = "A-01-02-01",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 2,
            //        PositionNumber = 2,
            //        FullLocationBarcode = "A-01-02-02",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 2,
            //        PositionNumber = 3,
            //        FullLocationBarcode = "A-01-02-03",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 2,
            //        PositionNumber = 4,
            //        FullLocationBarcode = "A-01-02-04",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 2,
            //        PositionNumber = 5,
            //        FullLocationBarcode = "A-01-02-05",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 3,
            //        PositionNumber = 1,
            //        FullLocationBarcode = "A-01-03-01",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 3,
            //        PositionNumber = 2,
            //        FullLocationBarcode = "A-01-03-02",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 3,
            //        PositionNumber = 3,
            //        FullLocationBarcode = "A-01-03-03",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 3,
            //        PositionNumber = 4,
            //        FullLocationBarcode = "A-01-03-04",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 3,
            //        PositionNumber = 5,
            //        FullLocationBarcode = "A-01-03-05",
            //        Pickable = true,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 4,
            //        PositionNumber = 1,
            //        FullLocationBarcode = "A-01-04-01",
            //        Pickable = false,
            //        Created = DateTime.Now
            //    },

            //    new Locations_Positions
            //    {
            //        Locations_ShelfsID = 4,
            //        PositionNumber = 2,
            //        FullLocationBarcode = "A-01-04-02",
            //        Pickable = false,
            //        Created = DateTime.Now
            //    }
            //);

            //context.Product_Locations.AddRange(
            //    new Product_Locations
            //    {
            //        Locations_PositionsID = null,
            //        ProductID = 1,
            //        Quantity = 1000,
            //        LocationBarcode = "Receiving-Station",
            //        Created = DateTime.Now
            //    },
                
            //    new Product_Locations
            //    {
            //        Locations_PositionsID = null,
            //        ProductID = 2,
            //        Quantity = 1000,
            //        LocationBarcode = "Receiving-Station",
            //        Created = DateTime.Now
            //    },

            //     new Product_Locations
            //    {
            //        Locations_PositionsID = null,
            //        ProductID = 3,
            //        Quantity = 1000,
            //        LocationBarcode = "Receiving-Station",
            //        Created = DateTime.Now
            //    }
            //);

            context.ReceiveRejectedReasons.AddRange(
                new ReceiveRejectedReasons
                {
                    Reason = "Damaged",
                    Created = DateTime.Now
                },

                new ReceiveRejectedReasons
                {
                    Reason = "Water Damage",
                    Created = DateTime.Now
                },

                new ReceiveRejectedReasons
                {
                    Reason = "Wrong Color",
                    Created = DateTime.Now
                },

                new ReceiveRejectedReasons
                {
                    Reason = "Dirty",
                    Created = DateTime.Now
                },

                new ReceiveRejectedReasons
                {
                    Reason = "Other",
                    Created = DateTime.Now
                }
            );

            context.SaveChanges();
        }
    }
}
