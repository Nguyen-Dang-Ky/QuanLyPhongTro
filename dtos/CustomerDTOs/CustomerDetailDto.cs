using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.CustomerDTOs
{
    public class CustomerDetailDto
    {
        public int CustomerID { get; set; }

        public int UserID { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int RoomID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public string IdentityNumber { get; set; } = string.Empty;

        public string PermanentAddress { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; }

        public int ContractCount { get; set; }
    }
}