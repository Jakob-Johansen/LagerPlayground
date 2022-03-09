using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceivingBox_Items
    {
        public int ID { get; set; }
        public int ReceivingBoxID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public ReceivingBox ReceivingBox { get; set; }
        public Product Product { get; set; }
    }
}
