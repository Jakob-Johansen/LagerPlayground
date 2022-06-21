using LagerPlayground.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order_Details> Order_Details { get; set; }
        public DbSet<Order_Items> Order_Items { get; set; }
        public DbSet<Custommer> Custommers { get; set; }
        public DbSet<Tote> Totes { get; set; }
        public DbSet<ReceivingBox> ReceivingBoxes { get; set; }
        public DbSet<ReceivingOrder_Details> ReceivingOrder_Details { get; set; }
        public DbSet<ReceivingOrder_Items> ReceivingOrder_Items { get; set; }
        public DbSet<ReceiveCustommer> ReceiveCustommers { get; set; }
        public DbSet<ReceivingBox_Items> ReceiveBox_Items { get; set; }
        public DbSet<ReceiveRejected> ReceiveRejecteds { get; set; }
        public DbSet<ReceiveRejectedReasons> ReceiveRejectedReasons { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Locations_Racks> Locations_Racks { get; set; }
        public DbSet<Locations_Shelfs> Locations_Shelfs { get; set; }
        public DbSet<Locations_Positions> Locations_Positions { get; set; }
        public DbSet<Product_Locations> Product_Locations { get; set; }

        // https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations_Positions>()
                .Property(x => x.Pickable)
                .HasDefaultValue(true);

            modelBuilder.Entity<Order_Items>()
                .Property(x => x.PickingQuantity)
                .HasDefaultValue(0);

            modelBuilder.Entity<Tote>()
                .Property(x => x.InUse)
                .HasDefaultValue(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
