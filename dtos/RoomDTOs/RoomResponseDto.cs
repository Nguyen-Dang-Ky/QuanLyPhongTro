using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.RoomDTOs
{
    public class RoomResponseDto
    {
        public int RoomID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public RoomStatus Status { get; set; }

        public int CurrentTenants { get; set; }

        public int MaxTenants { get; set; }

        public string BoardingHouseName { get; set; } = string.Empty;
    }
}