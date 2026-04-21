using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class BillDetails
    {
        [Key]
        public int BillDetailID {get;set;}

        [Required]
        public int BillID {get;set;}

        [Column(TypeName = "nvarchar(100)")]
        public string Description {get;set;} = string.Empty; // e.g., "Room rent for September 2024", "Electricity bill for September 2024", etc.

        [Column(TypeName = "char(30)")]
        public string Quantity {get;set;} = string.Empty; // e.g., "1 month", "50 kWh", etc.

        [Column(TypeName = "decimal(8,2)")]
        public decimal UnitPrice {get;set;} = 0;

        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalPrice {get;set;} = 0;

        // Navigation N:1
        [ForeignKey("BillID")]
        public virtual Bills? Bill {get;set;}
    }
}