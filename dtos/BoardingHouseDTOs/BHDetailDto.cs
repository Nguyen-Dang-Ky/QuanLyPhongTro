using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.BoardingHouseDTOs
{
    public class BHDetailDto
    {
        public int BoardingHouseID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateOnly CreatedAt { get; set; }

        // Owner info
        public int OwnerID { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        public int RoomCount { get; set; }
        public int ServiceCount { get; set; }
    }
}