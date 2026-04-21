using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class BoardingHouse
    {
        [Key]
        public int BoardingHouseID {get;set;}

        
        [Required]
        public int OwnerID {get;set;}

        [Column(TypeName = "nvarchar(200)")]
        public string Name {get;set;} = string.Empty;

        [Column(TypeName = "nvarchar(200)")]
        public string Address {get;set;} = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string Description {get;set;} = string.Empty;

        [Column(TypeName = "date")]
        public DateOnly CreatedAt {get;set;} = DateOnly.FromDateTime(DateTime.UtcNow);
        public bool IsDeleted { get; set; } = false;

        // Navigation N:1 
        [ForeignKey("OwnerID")]
        public virtual User? Owner {get;set;}

        public virtual ICollection<Rooms> Rooms {get;set;} = new List<Rooms>();
        public virtual ICollection<Services> Services {get;set;} = new List<Services>();
    }
}