﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanDienThoai2
{
    internal class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
