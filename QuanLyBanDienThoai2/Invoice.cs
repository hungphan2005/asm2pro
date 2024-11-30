using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanDienThoai2
{
    internal class Invoice
    {
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
