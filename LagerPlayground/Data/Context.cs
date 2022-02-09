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
        public DbSet<ReceiveBox_Items> ReceiveBox_Items { get; set; }
    }
}
