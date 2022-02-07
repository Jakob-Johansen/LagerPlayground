using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceiveStatus
    {
        public int ID { get; set; }
        public int ReceivingOrder_DetailsID { get; set; }
        public int ReceivingOrder_ItemsID { get; set; }
        public int Rejected { get; set; }
        public int Accepted { get; set; }
        public int Unreceived { get; set; }

        public ReceivingOrder_Items ReceivingOrder_Items { get; set; }
        public ReceivingOrder_Details ReceivingOrder_Details { get; set; }
    }
}
