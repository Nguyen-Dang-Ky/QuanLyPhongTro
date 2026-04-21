using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public enum RoomStatus
    {
        Available = 0,
        Occupied = 1,
        Maintenance = 2
    }
    public class Rooms
    {
        [Key]
        public int RoomID {get;set;}

        [Required]
        public int BoardingHouseID {get;set;}

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string RoomNumber {get;set;} = string.Empty;

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price {get;set;} = 0;

        // [Column(TypeName = "varchar(30)")]
        // public string Status {get;set;} = "Available"; // Available, Occupied, Maintenance
        public RoomStatus Status {get;set;} = RoomStatus.Available;

        public int Maxtenants {get;set;} = 2;
        public DateOnly CreatedAt {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        // Navigation N:1 ---------------
        [ForeignKey("BoardingHouseID")]
        public virtual BoardingHouse? BoardingHouse {get;set;}


        // Navigation 1:N ---------------
        public virtual ICollection<Customers> Customers {get;set;} = new List<Customers>();


        // Navigation 1:N ---------------
        // With CONTRACTS
        public virtual ICollection<Contracts> Contracts {get;set;} = new List<Contracts>();
        // With BILLS
        public virtual ICollection<Bills> Bills {get;set;} = new List<Bills>();
        // With SERVICES USAGES
        public virtual ICollection<ServiceUsages> ServiceUsages {get;set;} = new List<ServiceUsages>();
    }
}