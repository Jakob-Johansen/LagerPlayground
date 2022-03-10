using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Locations_details
    {
        public int ID { get; set; }
        public int LocationsID { get; set; }
        public int? Shelfs { get; set; }
        public int? Bins { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public Locations Locations { get; set; }
    }
}
