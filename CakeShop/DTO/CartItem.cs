using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DTO
{
    class CartItem
    {
        public string Id { get; set; }
        public Cake Cake { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
