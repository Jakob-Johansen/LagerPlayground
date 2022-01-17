using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Order_Details
    {
        public int ID { get; set; }
        public int CustommerID { get; set; }
        public int Order_ItemsID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public Custommer Custommer { get; set; }
        public ICollection<Order_Items> Order_Items { get; set; }
    }
}
