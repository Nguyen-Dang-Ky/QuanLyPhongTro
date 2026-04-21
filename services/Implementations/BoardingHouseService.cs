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

namespace QuanLyPhongTro.services.Implementations
{
    public class BoardingHouseService : IBoardingHouseService
    {
        private readonly ManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BoardingHouseService(
            ManagementDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // ==============
        // Get all Boarding House
        // ==============
        public async Task<IEnumerable<BHResponseDto>> GetBoardingHousesAsync()
        {
            var boardingHouses = _context.BoardingHouses.ToList();
            return boardingHouses.Select(bh => new BHResponseDto
            {
                BoardingHouseID = bh.BoardingHouseID,
                Name = bh.Name,
                Address = bh.Address,
                CreatedAt = bh.CreatedAt
            });
        }
        
        // ==============
        // Get Boarding House by ID
        // ==============
        public async Task<BHResponseDto?> GetBoardingHouseByIdAsync(int id)
        {
            var bh = _context.BoardingHouses.FirstOrDefault(b => b.BoardingHouseID == id);
            if (bh == null) return null;

            return new BHResponseDto
            {
                BoardingHouseID = bh.BoardingHouseID,
                Name = bh.Name,
                Address = bh.Address,
                CreatedAt = bh.CreatedAt
            };
        }

        // ==============
        // Create Boarding House
        // ==============
        public async Task<BHResponseDto> CreateBoardingHouseAsync(BHCreateDto createDto)
        {
            // Lấy OwnerID từ JWT
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if(userIdClaim == null)
                throw new UnauthorizedAccessException("User not Authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // Tạo entity
            var boardingHouse = new BoardingHouse
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Description = createDto.Description 
            };

            // Luu vao DB
            _context.BoardingHouses.Add(boardingHouse);
            await _context.SaveChangesAsync();

            // Map sang ResponseDto
            return new BHResponseDto
            {
                BoardingHouseID = boardingHouse.BoardingHouseID,
                Name = boardingHouse.Name,
                Address = boardingHouse.Address,
                Description = boardingHouse.Description
            };
        }

        // ==============
        // Update Boarding House
        // ==============
        public async Task<BHResponseDto> UpdateBoardingHouseAsync(int id, BHUpdateDto updateDto)
        {
            // 1️⃣ Lấy OwnerId từ JWT
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // 2️⃣ Tìm BoardingHouse
            var boardingHouse = await _context.BoardingHouses
                .FirstOrDefaultAsync(b => b.BoardingHouseID == id);

            if (boardingHouse == null)
                throw new KeyNotFoundException("Boarding house not found.");

            // 3️⃣ Kiểm tra quyền
            if (boardingHouse.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to update this boarding house.");

            // 4️⃣ Update dữ liệu
            boardingHouse.Name = updateDto.Name;
            boardingHouse.Address = updateDto.Address;
            boardingHouse.Description = updateDto.Description;

            await _context.SaveChangesAsync();

            // 5️⃣ Return DTO
            return new BHResponseDto
            {
                BoardingHouseID = boardingHouse.BoardingHouseID,
                Name = boardingHouse.Name,
                Address = boardingHouse.Address,
                Description = boardingHouse.Description
            };
        }


        // ==============
        // Delete Boarding House
        // ==============
        public async Task<bool> DeleteBoardingHouseAsync(int id)
        {
            // 1️⃣ Lấy OwnerId từ JWT
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // 2️⃣ Tìm BoardingHouse
            var boardingHouse = await _context.BoardingHouses
                .FirstOrDefaultAsync(b => b.BoardingHouseID == id && b.OwnerID == ownerId);

            if (boardingHouse == null)
                throw new KeyNotFoundException("Boarding house not found.");

            // 3️⃣ Kiểm tra quyền
            if (boardingHouse.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to delete this boarding house.");

            // 4️⃣ Xóa
            boardingHouse.IsDeleted = true;
            
            await _context.SaveChangesAsync();

            return true;
        }
    }
}