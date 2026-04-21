using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.UserDTOs;
using QuanLyPhongTro.model;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using BCrypt.Net;

namespace QuanLyPhongTro.services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ManagementDbContext _context;
        public UserService(ManagementDbContext context)
        {
            _context = context;
        }

        // =============
        // Get all Users
        // =============
        public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
        {
            var users = _context.Users.ToList();
            return users.Select(u => new UserResponseDto
            {
                UserID = u.UserID,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });
        }

        // =============
        // Get user by ID
        // =============
        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null) return null;

            return new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        // =============
        // Create user
        // =============
        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto createUserDto)
        {
            var user = new User
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Password = createUserDto.Password,
                Role = createUserDto.Role,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        // =============
        // Update user
        // =============
        public async Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateDto updateUserDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrEmpty(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (!string.IsNullOrEmpty(updateUserDto.Email))
                user.Email = updateUserDto.Email;

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        // =============
        // Delete user (soft delete)
        // =============
        public async Task<DeleteUserResult> DeleteUserAsync(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null) return DeleteUserResult.UserNotFound;

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return DeleteUserResult.Success;
        }

        // =============
        // Change password using BCrypt
        // =============
        public async Task<ChangePasswordResult> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (user == null) return ChangePasswordResult.UserNotFound;

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
                return ChangePasswordResult.WrongOldPassword;

            if (dto.NewPassword != dto.NewPassword)
                return ChangePasswordResult.PasswordMismatch;

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ChangePasswordResult.Success;
        }
        

        
    }
}