namespace LagerPlayground.Models
{
    public class Picking_Info
    {
        public int ID { get; set; }
        public int Order_DetailsID { get; set; }
        public int Order_ItemsID { get; set; }
        public string ToteSku { get; set; }
        public int Quantity { get; set; }
        public bool Completed { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public Order_Details Order_Details { get; set; }
        public IEnumerable<Order_Items> Order_Items { get; set; }
    }
}
