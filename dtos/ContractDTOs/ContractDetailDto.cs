using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.ContractDTOs
{
    public class ContractDetailDto
    {
        public int ContractID { get; set; }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal MonthlyPrice { get; set; }

        public ContractStatus Status { get; set; }

        public DateOnly CreatedAt { get; set; }
    }
}