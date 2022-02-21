using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class ReceiveRejected
    {
        public int ID { get; set; }
        public int ReceivingOrder_ItemsID { get; set; }
        public int Quantity { get; set; }
        public int ReceiveRejectedReasonID { get; set; }
        public string Note { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        [NotMapped]
        public ReceiveRejectedReasons ReceiveRejectedReason { get; set; }

        //[NotMapped]
        //public IEnumerable<ReceiveRejectedReasons> ReceiveRejectedReasons { get; set; }

        [NotMapped]
        public  ReceivingOrder_Items ReceivingOrder_Items { get; set; }
    }
}
