using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models.VM
{
    public class VMShelfAndPositions
    {
        public int ShelfID { get; set; }
        public int ShelfNumber { get; set; }
        public int PositionLength { get; set; }
        public List<VMPositions> VMPositions { get; set; }
    }
}
