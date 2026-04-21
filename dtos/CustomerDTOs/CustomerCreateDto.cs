using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.CustomerDTOs
{
    public class CustomerCreateDto
    {
        public int UserID { get; set; }

        public int RoomID { get; set; }

        public string IdentityNumber { get; set; } = string.Empty;

        public string PermanentAddress { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}