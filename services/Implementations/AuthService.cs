using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.UserDTOs;
using QuanLyPhongTro.model;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTro.services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            ManagementDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            // 1️⃣ Tìm user theo email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // 2️⃣ Kiểm tra tồn tại + verify mật khẩu
            if (user == null || 
                !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // 3️⃣ Tạo JWT token
            var token = GenerateJwtToken(user);

            // 4️⃣ Map sang UserResponseDto
            var userDto = new UserResponseDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

            // 5️⃣ Return AuthResponseDto
            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("id", user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}