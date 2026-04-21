using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public enum BillStatus
    {
        Unpaid=0,
        Paid=1,
        Overdue=2
    }
    public class Bills
    {
        [Key]
        public int BillID {get;set;}

        [Required]
        public int RoomID {get;set;}

        public int Month {get;set;}
        public int Year {get;set;}

        [Column(TypeName = "decimal(8,2)")]
        public decimal RoomCharge {get;set;} = 0;
        [Column(TypeName = "decimal(8,2)")]
        public decimal ServiceCharge {get;set;} = 0;
        [Column(TypeName = "decimal(8,2)")]
        public decimal OtherCharges {get;set;} = 0;
        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalAmount {get;set;} = 0;

        public BillStatus Status {get;set;} = BillStatus.Unpaid;

        [Column(TypeName = "date")]
        public DateOnly CreatedAt {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);
        [Column(TypeName = "date")]
        public DateOnly DueDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(7);


        // Navigation N:1
        [ForeignKey("RoomID")]
        public virtual Rooms? Room {get;set;}

        // Navigation 1:N
        public virtual ICollection<BillDetails> BillDetails {get;set;} = new List<BillDetails>();

        public virtual ICollection<Payments> Payments {get;set;} = new List<Payments>();
    }
}