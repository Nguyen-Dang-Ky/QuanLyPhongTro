
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.CustomerDTOs
{
    public class CustomerResponseDto
    {
        public int CustomerID { get; set; }

        public int UserID { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int RoomID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}