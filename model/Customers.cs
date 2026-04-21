using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class Customers
    {
        [Key]
        public int CustomerID {get;set;}

        [Required]
        public int UserID {get;set;}

        [Required]
        public int RoomID {get;set;}

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string IdentityNumber {get;set;} = string.Empty;

        [Column(TypeName = "varchar(200)")]
        public string PermanentAddress {get;set;} = string.Empty;

        [Column(TypeName = "date")]
        public DateOnly StartDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "date")]
        public DateOnly EndDate {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);

        public bool IsActive {get;set;} = true;

        // Navigation 1:1
        [ForeignKey("UserID")]
        public virtual User? User {get;set;}

        public virtual ICollection<Contracts> Contracts {get;set;} = new List<Contracts>();

        [ForeignKey("RoomID")]
        public virtual Rooms? Rooms {get;set;}
    }
}