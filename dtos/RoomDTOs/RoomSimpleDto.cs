using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.RoomDTOs
{
    public class RoomSimpleDto
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
    }
}