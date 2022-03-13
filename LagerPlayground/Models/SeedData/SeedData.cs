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
                    Quantity = 0
                },

                new Product
                {
                    Name = "ModMic USB",
                    Description = "En Mikrofon",
                    BrandName = "ANTLION AUDIO",
                    Category = "Elektronik",
                    BarcodeID = "761878978616",
                    Image = "2804253_5ebbb392ba6f.jpg",
                    Quantity = 0
                },

                new Product
                {
                    Name = "ZINZINO XTEND+",
                    Description = "Vitaminer",
                    BrandName = "ZINZINO",
                    Category = "Fødevare",
                    BarcodeID = "302205",
                    Image = "zinzino-xtend-natural-essential-vitamins-minerals2.png",
                    Quantity = 0
                }
            );

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
