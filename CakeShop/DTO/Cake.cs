using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.DTO
{
    class Cake
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image_Main { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
