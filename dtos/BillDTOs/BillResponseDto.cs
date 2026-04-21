using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.BillDTOs
{
    public class BillResponseDto
    {
        public int BillID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal TotalAmount { get; set; }

        public BillStatus Status { get; set; }

        public DateOnly DueDate { get; set; }
    }
}