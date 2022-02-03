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
        public DateTime ArrivalDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public ReceiveCustommer ReceiveCustommer { get; set; }
    }
}
