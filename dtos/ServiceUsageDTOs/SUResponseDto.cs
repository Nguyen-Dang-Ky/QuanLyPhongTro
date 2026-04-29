using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceUsageDTOs
{
    public class SUResponseDto
    {
        public int ServiceUsageID { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string RoomNumber { get; set; } = string.Empty;

        public int UsageMonth {get;set;}
        public int UsageYear {get;set;}

        public string Quantity { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
    }
}