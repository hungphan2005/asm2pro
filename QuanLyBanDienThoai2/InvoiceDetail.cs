using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanDienThoai2
{
    internal class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        public Invoice Invoice { get; set; }
    }
}
