using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceUsageDTOs
{
    public class SUDetailDto
    {
        public int ServiceUsageID { get; set; }

        public int ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal ServicePrice { get; set; }

        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;

        public int UsageMonth {get;set;}
        public int UsageYear {get;set;}

        public string OldIndex { get; set; } = string.Empty;
        public string NewIndex { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
    }
}