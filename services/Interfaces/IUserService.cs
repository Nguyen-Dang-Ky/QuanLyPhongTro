using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.UserDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public enum ChangePasswordResult
    {
        Success,
        UserNotFound,
        WrongOldPassword,
        PasswordMismatch
    }

    public enum DeleteUserResult
    {
        Success,
        UserNotFound,
        HasAssociatedRooms
    }
    
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<UserResponseDto> CreateUserAsync(UserCreateDto createUserDto);
        Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateDto updateUserDto);
        Task<DeleteUserResult> DeleteUserAsync(int userId);
        Task<ChangePasswordResult> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }

}