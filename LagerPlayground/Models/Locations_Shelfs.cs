using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Locations_Shelfs
    {
        public int ID { get; set; }
        public int Locations_RacksID { get; set; }
        public int ShelfNumber { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public Locations_Racks Locations_Rack { get; set; }
        public IEnumerable<Locations_Positions> Locations_Positions { get; set; }
    }
}
