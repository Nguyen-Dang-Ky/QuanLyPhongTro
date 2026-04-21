using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.BoardingHouseDTOs
{
    public class BHResponseDto
    {
        public int BoardingHouseID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateOnly CreatedAt { get; set; }

        public int RoomCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}