using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.dtos.RoomDTOs
{
    public class RoomDetailDto
    {
        public int RoomID { get; set; }

        public int BoardingHouseID { get; set; }
        public string BoardingHouseName { get; set; } = string.Empty;

        public string RoomNumber { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public RoomStatus Status { get; set; }

        public int MaxTenants { get; set; }

        public int CurrentTenants { get; set; }

        public int ContractCount { get; set; }

        public int BillCount { get; set; }

        public DateOnly CreatedAt { get; set; }
    }
}