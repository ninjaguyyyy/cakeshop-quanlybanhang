using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DTO
{
    class Order
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public List<CartItem> CartItems { get; set; }
        public string CreatedDate { get; set; }
        public int TotalPrice { get; set; }
    }
}
