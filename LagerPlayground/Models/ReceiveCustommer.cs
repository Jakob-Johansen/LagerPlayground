using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceiveCustommer
    {
        public int ID { get; set; }
        public string Vendor { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public ReceivingOrder_Details ReceivingOrder_Details { get; set; }
    }
}
