using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.CustomerDTOs
{
    public class CustomerSimpleDto
    {
        public int CustomerID { get; set; }

        public string CustomerName { get; set; } = string.Empty;
    }
}