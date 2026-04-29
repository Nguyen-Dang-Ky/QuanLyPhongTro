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
            return await _context.BoardingHouses
            .AsNoTracking()
            .Select(bh => new BHResponseDto
            {
                BoardingHouseID = bh.BoardingHouseID,
                Name = bh.Name,
                Address = bh.Address,
                CreatedAt = bh.CreatedAt,
                Description = bh.Description,
                RoomCount = bh.Rooms.Count()
            })
            .ToListAsync();
        }
        
        // ==============
        // Get Boarding House by ID
        // ==============
        public async Task<BHResponseDto?> GetBoardingHouseByIdAsync(int id)
        {
            var boardingHouse = await _context.BoardingHouses
                .AsNoTracking()
                .Where(bh => bh.BoardingHouseID == id)
                .Select(bh => new BHResponseDto
                {
                    BoardingHouseID = bh.BoardingHouseID,
                    Name = bh.Name,
                    Address = bh.Address,
                    CreatedAt = bh.CreatedAt,
                    Description = bh.Description,
                    RoomCount = bh.Rooms.Count()
                })
                .FirstOrDefaultAsync();

            return boardingHouse;
        }

        // ==============
        // Create Boarding House
        // ==============
        public async Task<BHResponseDto> CreateBoardingHouseAsync(BHCreateDto createDto)
        {
            // Lấy OwnerID từ JWT
            var userIdClaim =  _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if(userIdClaim == null)
                throw new UnauthorizedAccessException("User not Authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // Tạo entity
            var boardingHouse = new BoardingHouse
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Description = createDto.Description,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                OwnerID = ownerId
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
                Description = boardingHouse.Description,
                CreatedAt = boardingHouse.CreatedAt,
                RoomCount = 0
            };
        }

        // ==============
        // Update Boarding House
        // ==============
        public async Task<BHResponseDto> UpdateBoardingHouseAsync(int id, BHUpdateDto updateDto)
        {
            // Tim Boarding House
            var boardinghouse = await _context.BoardingHouses
                .Include(bh => bh.Rooms)
                .FirstOrDefaultAsync(bh => bh.BoardingHouseID == id);
            if(boardinghouse == null) throw new Exception("Boarding house not found");

            var userClaimId = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;
            
            if(string.IsNullOrEmpty(userClaimId))
                throw new UnauthorizedAccessException("User not authenticated");
            int currentUserId = int.Parse(userClaimId);

            bool isAdmin = _httpContextAccessor.HttpContext!
                .User.IsInRole("Admin");
            
            if(boardinghouse.OwnerID != currentUserId && !isAdmin)
                throw new UnauthorizedAccessException("You do not have permission to update this boarding house");

            // UPDATE tung field neu co
            if(!string.IsNullOrWhiteSpace(updateDto.Name))
                boardinghouse.Name = updateDto.Name;
            if(!string.IsNullOrWhiteSpace(updateDto.Address))
                boardinghouse.Address = updateDto.Address;
            if(!string.IsNullOrWhiteSpace(updateDto.Description))
            boardinghouse.Description = updateDto.Description;

            // Luu vao DB
            await _context.SaveChangesAsync();

            //Tra ve DTO
            return new BHResponseDto
            {
                BoardingHouseID = boardinghouse.BoardingHouseID,
                Name = boardinghouse.Name,
                Address = boardinghouse.Address,
                Description = boardinghouse.Description,
                CreatedAt = boardinghouse.CreatedAt,
                RoomCount = boardinghouse.Rooms.Count
            };
        }


        // ==============
        // Delete Boarding House
        // ==============
        public async Task<bool> DeleteBoardingHouseAsync(int id)
        {
            // 1️⃣ Tìm BoardingHouse
            var boardingHouse = await _context.BoardingHouses
                .FirstOrDefaultAsync(bh => bh.BoardingHouseID == id);

            if (boardingHouse == null)
                return false;

            // 2️⃣ Lấy userId từ JWT
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated");

            int currentUserId = int.Parse(userIdClaim);

            // 3️⃣ Kiểm tra quyền (Owner hoặc Admin)
            bool isAdmin = _httpContextAccessor.HttpContext!
                .User.IsInRole("Admin");

            if (boardingHouse.OwnerID != currentUserId && !isAdmin)
                throw new UnauthorizedAccessException("You do not have permission to delete this boarding house");

            // 4️⃣ Soft delete
            if (boardingHouse.IsDeleted)
                return false;

            boardingHouse.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}