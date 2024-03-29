﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Order_Items
    {
        public int ID { get; set; }
        public int Order_DetailsID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int PickingQuantity { get; set; }
        public string PickingToteBarcode { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        [JsonIgnore]
        public Order_Details Order_Details { get; set; }
        public Product Product { get; set; }
    }
}
