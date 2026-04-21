using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public enum ContractStatus
    {
        Active = 0,
        Expired = 1,
        Cancelled = 2
    }
    public class Contracts
    {
        [Key]
        public int ContractID {get;set;}

        [Required]
        public int CustomerID {get;set;}

        [Required]
        public int RoomID {get;set;}

        [Column(TypeName = "date")]
        public DateOnly StartDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "date")]
        public DateOnly EndDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "decimal(8,2)")]
        public decimal DepositAmount {get;set;} = 0;

        [Column(TypeName = "decimal(8,2)")]
        public decimal MonthlyPrice {get;set;} = 0;

        // [Column(TypeName = "varchar(20)")]
        // public string Status {get;set;} = "Active"; // Active, Terminated, Pending
        public ContractStatus Status {get;set;} = ContractStatus.Active;

        [Column(TypeName = "date")]
        public DateOnly CreatedAt {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        // Navigation 1:1
        [ForeignKey("CustomerID")]
        public virtual Customers? Customer {get;set;}

        // Navigation N:1
        [ForeignKey("RoomID")]
        public virtual Rooms? Room {get;set;}
    }
}