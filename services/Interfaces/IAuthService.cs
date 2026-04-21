using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.UserDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto);
    }
}