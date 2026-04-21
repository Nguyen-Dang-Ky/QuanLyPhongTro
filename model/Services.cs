using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTro.model
{
    public class Services
    {
        [Key]
        public int ServiceID {get;set;}

        [Required]
        public int BoardingHouseID {get;set;}

        [Column(TypeName = "nvarchar(100)")]
        public string ServiceName {get;set;} = string.Empty;

        [Column(TypeName = "char(30)")]
        public string Unit {get;set;} = string.Empty;

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price {get;set;} = 0;

        // IsActive: true = service is currently offered, false = service is discontinued
        public bool IsActive {get;set;} = true;

        
        // Navigation N:1 ---------------
        [ForeignKey("BoardingHouseID")]
        public virtual BoardingHouse? BoardingHouse {get;set;}

        // Navigation 1:N ---------------
        public virtual ICollection<ServiceUsages> ServiceUsages {get;set;} = new List<ServiceUsages>();
    }
}