using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Locations_Positions
    {
        public int ID { get; set; }
        public int Locations_ShelfsID { get; set; }
        public int PositionNumber { get; set; }
        public string FullLocationBarcode { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public Locations_Shelfs Locations_Shelf { get; set; }
    }
}
