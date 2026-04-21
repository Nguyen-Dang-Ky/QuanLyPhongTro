using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.ContractDTOs
{
    public class ContractSimpleDto
    {
        public int ContractID { get; set; }

        public ContractStatus Status { get; set; }
    }
}