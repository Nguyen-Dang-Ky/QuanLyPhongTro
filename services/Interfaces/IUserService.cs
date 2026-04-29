using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.UserDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IUserService
    {
        // ========================
        // AUTH
        // ========================

        Task<LoginDto?> LoginAsync(LoginDto request);
        Task<UserResponseDto> RegisterAsync(UserCreateDto request);

        // ========================
        // USER PROFILE
        // ========================

        Task<UserResponseDto?> GetByIdAsync(int userId);
        Task<List<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto?> UpdateUserAsync(int userId, UserUpdateDto request);

        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto request);

        // ========================
        // ADMIN MANAGEMENT
        // ========================

        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);

    }
}