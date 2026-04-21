using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.BillDetailDTOs
{
    public class BillDetailUpdateDto
    {
        public string Description { get; set; } = string.Empty;

        public string Quantity { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
    }
}