﻿namespace LagerPlayground.Models.VM
{
    public class DTOPickLocation
    {
        public int Order_DetailsID { get; set; }
        public int ProductID { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        public int PickQuantity { get; set; }
        public int OnHandQuantity { get; set; }
        public string LocationBarcode { get; set; }
        public string OrderStatus { get; set; }
        public string PickingToteBarcode { get; set; }
    }
}
