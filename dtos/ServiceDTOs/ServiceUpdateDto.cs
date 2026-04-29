using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceDTOs
{
    public class ServiceUpdateDto
    {
        public string ServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}