using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyPhongTro.dtos.ServiceUsageDTOs
{
    public class SUCreateDto
    {
        public int ServiceID { get; set; }

        public int RoomID { get; set; }

        public int UsageMonth {get;set;}
        public int UsageYear {get;set;}

        public int OldIndex {get;set;}
        public int NewIndex {get;set;}
    }
}