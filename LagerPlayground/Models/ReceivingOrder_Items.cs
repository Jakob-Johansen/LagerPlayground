using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Accepted { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        //[NotMapped]
        public int Rejected {
            get
            {
                int rejected = 0;
                if (ReceiveRejecteds != null)
                {
                    foreach (var item in ReceiveRejecteds)
                    {
                        if (item.Quantity != 0)
                        {
                            rejected += item.Quantity;
                        }
                    }
                }

                return rejected;
            }
        }

        //[NotMapped]
        public int Unreceived { 
            get
            {
                int unreceived = (Quantity- Rejected) - Accepted;

                if (unreceived < 0)
                    unreceived = 0;

                return unreceived;
            }
        }

        public ReceivingOrder_Details ReceivingOrder_Details { get; set; }
        public Product Product { get; set; }
        public IEnumerable<ReceiveRejected> ReceiveRejecteds { get; set; }
    }
}
