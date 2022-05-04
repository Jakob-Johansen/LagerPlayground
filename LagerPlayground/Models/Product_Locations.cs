using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Product_Locations
    {
        public int ID { get; set; }
        public int? Locations_PositionsID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string LocationBarcode { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }


        public Product Product { get; set; }
        //[System.Text.Json.Serialization.JsonIgnore]
        public Locations_Positions Locations_Positions { get; set; }
    }
}
