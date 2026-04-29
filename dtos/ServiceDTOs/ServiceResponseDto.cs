using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceDTOs
{
    public class ServiceResponseDto
    {
        public int ServiceID { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string BoardingHouseName { get; set; } = string.Empty;
    }
}