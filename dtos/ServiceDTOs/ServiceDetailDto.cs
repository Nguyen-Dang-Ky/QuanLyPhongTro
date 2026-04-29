using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceDTOs
{
    public class ServiceDetailDto
    {
        public int ServiceID { get; set; }

        public int BoardingHouseID { get; set; }

        public string BoardingHouseName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int UsageCount { get; set; }
    }
}