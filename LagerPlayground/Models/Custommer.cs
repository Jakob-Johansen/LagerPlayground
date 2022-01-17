using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Models
{
    public class Custommer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public Order_Details OrderDetail { get; set; }
    }
}
