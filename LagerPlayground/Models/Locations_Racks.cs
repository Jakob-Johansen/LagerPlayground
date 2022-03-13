using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Locations_Racks
    {
        // https://blog.barcodefactory.com/blog/warehouse-rack-labeling-best-practices
        public int ID { get; set; }
        public int LocationsID { get; set; }
        public int RackNumber { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public Locations Locations { get; set; }
    }
}
