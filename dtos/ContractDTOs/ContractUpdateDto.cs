using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.ContractDTOs
{
    public class ContractUpdateDto
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public decimal DepositAmount { get; set; }

        public decimal MonthlyPrice { get; set; }

        public ContractStatus Status { get; set; }
    }
}