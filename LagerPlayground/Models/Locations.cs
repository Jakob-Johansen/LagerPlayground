using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Locations
    {
        public int ID { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public bool Dynamic { get; set; }
        public string Warehouse { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public IEnumerable<Locations_details> Locations_Details { get; set; }
    }
}
