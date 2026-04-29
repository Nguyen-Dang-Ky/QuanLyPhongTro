using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.BoardingHouseDTOs;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using QuanLyPhongTro.model;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.dtos.UserDTOs;
using Microsoft.Identity.Client;
using BCrypt.Net;

namespace QuanLyPhongTro.services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ManagementDbContext _context;
        private readonly IJwtService _jwtService;

        public UserService(ManagementDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        // ========================
        // AUTH
        // ========================
        public async Task<LoginDto?> LoginAsync (LoginDto request)
        {
            //Tim User theo Email
            var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email
            );
            if(user == null) return null;

            //Kiem tra tai khoan co Active khong?
            if(!user.IsActive) return null;

            //Verify password = BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.Password
            );
            if(!isPasswordValid) return null;

            //Tao claim cho JWT
            var claims = new List<Claim>
            {
                new Claim("id", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            //  Generate token
            var token = _jwtService.GenerateToken(claims);

            // ⚠️ Vì bạn đang dùng LoginDto làm cả request & response
            // tạm thời return lại LoginDto (không nên trả password)
            return new LoginDto
            {
                Email = user.Email,
                Password = token
            };
        }
        public async Task<UserResponseDto> RegisterAsync (UserCreateDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ Kiểm tra email đã tồn tại chưa
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                    throw new InvalidOperationException("Email already exists");

                // 2️⃣ Hash password bằng BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // 3️⃣ Tạo entity User
                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = request.Role, // có thể ép Customer nếu muốn bảo mật hơn
                    IsActive = true,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (user.Role == UserRole.Customer)
                {
                    var customer = new Customers
                    {
                        UserID = user.UserID
                    };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();

                // 5️⃣ Trả về DTO (không trả password)
                return new UserResponseDto
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        // ========================
        // USER PROFILE
        // ========================
        public async Task<UserResponseDto?> GetByIdAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                return null;

            return new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(user => new UserResponseDto
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                })
                .ToListAsync();

            return users;
        }
        public async Task<UserResponseDto?> UpdateUserAsync(int userId, UserUpdateDto request)
        {
            // 1️⃣ Tìm user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                return null;

            // 2️⃣ Nếu cập nhật Email → kiểm tra trùng
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                request.Email != user.Email)
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == request.Email && u.UserID != userId);

                if (emailExists)
                    throw new Exception("Email already exists");
            }

            // 3️⃣ Update từng field nếu có giá trị
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (request.IsActive.HasValue)
                user.IsActive = request.IsActive.Value;

            // 4️⃣ Lưu DB
            await _context.SaveChangesAsync();

            // 5️⃣ Trả về DTO
            return new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto request)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserID == userId);
            if(user == null) return false;

            bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(
                request.OldPassword,
                user.Password
            );

            if(!isOldPasswordValid) return false;

            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.Password = hashedNewPassword;

            //Luu vao DB 
            await _context.SaveChangesAsync();
            return true;
        }
        // ========================
        // ADMIN MANAGEMENT
        // ========================
        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                return false;

            if (!user.IsActive)
                return false;

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                return false;

            if (user.IsActive)
                return false;

            user.IsActive = true;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}