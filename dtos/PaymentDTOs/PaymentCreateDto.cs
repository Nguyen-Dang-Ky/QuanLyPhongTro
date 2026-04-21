using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.PaymentDTOs
{
    public class PaymentCreateDto
    {
        public int BillID { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateOnly? PaymentDate { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
    }
}