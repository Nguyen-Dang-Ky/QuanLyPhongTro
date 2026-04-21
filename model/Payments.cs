using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class Payments
    {
        [Key]
        public int PaymentID {get;set;}

        [Required]
        public int BillID {get;set;}

        [Column(TypeName = "decimal(8,2)")]
        public decimal AmountPaid {get;set;} = 0;

        [Column(TypeName = "varchar(100)")]
        public string PaymentMethod {get;set;} = string.Empty; // e.g., "Cash", "Credit Card", "Bank Transfer", etc.

        [Column(TypeName = "date")]
        public DateOnly PaymentDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "nvarchar(200)")]
        public string TransactionCode {get;set;} = string.Empty; // e.g., "TXN123456789", etc.

        // Navigation N:1
        [ForeignKey("BillID")]
        public virtual Bills? Bill {get;set;}
    }
}