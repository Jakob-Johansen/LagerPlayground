using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models.VM
{
    public class DTOPack
    {
        public int Id { get; set; }
        public string Tote { get; set; }
        public int Items { get; set; }
        public IEnumerable<string> ProductImages { get; set; }
        public string Status { get; set; }
        public DateTime? ShipByDate { get; set; }
    }
}
