using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.RoomDTOs
{
    public class RoomCreateDto
    {
        public int BoardingHouseID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int MaxTenants { get; set; }
    }
}