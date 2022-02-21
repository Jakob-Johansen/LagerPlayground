using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceivingOrder_Details
    {
        public int ID { get; set; }
        public int ReceiveCustommerID { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? Expected { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Closed { get; set; }
        public DateTime? Modified { get; set; }

        public ReceiveCustommer ReceiveCustommer { get; set; }
        public ICollection<ReceivingOrder_Items> ReceivingOrder_Items { get; set; }
    }
}
