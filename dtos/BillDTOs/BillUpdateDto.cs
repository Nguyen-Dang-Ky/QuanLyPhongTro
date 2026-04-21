using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.BillDTOs
{
    public class BillUpdateDto
    {
        public decimal RoomCharge { get; set; }

        public decimal ServiceCharge { get; set; }

        public decimal OtherCharges { get; set; }

        public DateOnly DueDate { get; set; }
    }
}