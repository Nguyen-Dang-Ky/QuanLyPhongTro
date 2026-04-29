using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.model;
using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTro.dtos.UserDTOs
{
    public class UserUpdateDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public bool? IsActive { get; set; }
    }
}