using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.BillDTOs
{
    public class BillDetailDto
    {
        public int BillID { get; set; }

        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;

        public int Month { get; set; }
        public int Year { get; set; }

        public decimal RoomCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal OtherCharges { get; set; }

        public decimal TotalAmount { get; set; }

        public BillStatus Status { get; set; }

        public DateOnly CreatedAt { get; set; }
        public DateOnly DueDate { get; set; }

        public int BillDetailCount { get; set; }
        public int PaymentCount { get; set; }
    }
}