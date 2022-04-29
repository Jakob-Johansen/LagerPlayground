using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models.VM
{
    public class VMPutawayGetProductFromLocation
    {
        public Product_Locations Product_Locations { get; set; }
        public int AllSkus { get; set; }
        public int AllUnits { get; set; }
    }
}
