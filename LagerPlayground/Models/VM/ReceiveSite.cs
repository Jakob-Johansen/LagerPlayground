using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models.VM
{
    public class ReceiveSite
    {
        public ReceivingOrder_Details ReceivingOrder_Details  { get; set; }
        public IEnumerable<ReceiveRejectedReasons> ReceiveRejectedReasons { get; set; }
    }
}
