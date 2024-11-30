using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanDienThoai2
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }
        //Foreign Key 
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
    }
}
