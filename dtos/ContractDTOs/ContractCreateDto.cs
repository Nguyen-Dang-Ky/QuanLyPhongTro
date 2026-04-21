using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ContractDTOs
{
    public class ContractCreateDto
    {
        public int CustomerID { get; set; }

        public int RoomID { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public decimal DepositAmount { get; set; }

        public decimal MonthlyPrice { get; set; }
    }
}