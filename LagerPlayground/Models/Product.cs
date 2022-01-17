using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Product
    {
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Barcode-ID")]
        public string BarcodeID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BrandName { get; set; }
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string Image { get; set; }
    }
}
