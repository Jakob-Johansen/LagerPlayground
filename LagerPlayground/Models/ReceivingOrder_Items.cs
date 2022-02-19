using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceivingOrder_Items
    {
        public int ID { get; set; }
        public int ReceivingOrder_DetailsID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int Rejected { get; set; }
        public int Accepted { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public int Unreceived { 
            get
            {

                int unreceived = Quantity - Accepted;

                if (unreceived < 0)
                    unreceived = 0;

                return unreceived;
            }
        }

        public ReceivingOrder_Details ReceivingOrder_Details { get; set; }
        public Product Product { get; set; }
    }
}
