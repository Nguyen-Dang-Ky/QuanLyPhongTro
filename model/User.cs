using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public enum UserRole
    {
        Admin = 0,
        Customer = 1
    }
    public class User
    {
        [Key]
        public int UserID {get;set;}

        [Column(TypeName = "nvarchar(100)")]
        public string FullName {get;set;} = string.Empty;

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string Email {get;set;} = string.Empty;

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber {get;set;} = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Password {get;set;} = string.Empty;

        // [Column(TypeName = "varchar(20)")]
        // public string Role {get;set;} = "KhachHang";

        public UserRole Role {get;set;} = UserRole.Customer;


        public Boolean IsActive {get;set;} = true;
        public DateOnly CreatedAt {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);


        // Navigation 1:1
        public virtual Customers? Customers {get;set;}
        // Navigation 1:N
        public virtual ICollection<BoardingHouse> BoardingHouses {get;set;} = new List<BoardingHouse>();
    }
}