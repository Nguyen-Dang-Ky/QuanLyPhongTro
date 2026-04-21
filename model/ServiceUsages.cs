using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class ServiceUsages
    {
        [Key]
        public int ServiceUsageID {get;set;}

        [Required]
        public int ServiceID {get;set;}

        [Required]
        public int RoomID {get;set;}

        public int UsageMonth {get;set;}
        public int UsageYear {get;set;}

        [Column(TypeName = "char(50)")]
        public string OldIndex {get;set;} = string.Empty;

        [Column(TypeName = "char(50)")]
        public string NewIndex {get;set;} = string.Empty;

        [Column(TypeName = "char(50)")]
        public string Quantity {get;set;} = string.Empty;

        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalPrice {get;set;} = 0;

        // Navigation N:1
        [ForeignKey("ServiceID")]
        public virtual Services? Service {get;set;}

        [ForeignKey("RoomID")]
        public virtual Rooms? Room {get;set;}
    }
}