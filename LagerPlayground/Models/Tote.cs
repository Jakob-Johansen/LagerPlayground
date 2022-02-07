using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Tote
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int Number { get; set; }
        public string Warehouse { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
