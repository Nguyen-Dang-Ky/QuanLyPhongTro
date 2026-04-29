using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceDTOs
{
    public class ServiceSimpleDto
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}