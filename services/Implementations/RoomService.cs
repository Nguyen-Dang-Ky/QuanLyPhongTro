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
using QuanLyPhongTro.dtos.RoomDTOs;
using System.Diagnostics;
using AutoMapper;

namespace QuanLyPhongTro.services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly ManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoomService (
            ManagementDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllRoomAsync()
        {
            var room = await _context.Rooms
                .Include(r => r.BoardingHouse)
                // .Include(r => r.Bills)
                // .Include(r => r.Contracts)
                .Include(r => r.Customers)
                .ToListAsync();

            var result = room.Select(r => new RoomResponseDto
            {
                RoomID = r.RoomID,
                RoomNumber = r.RoomNumber,
                Price = r.Price,
                Status = r.Status,
                CurrentTenants = r.Customers?.Count ?? 0,
                MaxTenants = r.Maxtenants,
                BoardingHouseName = r.BoardingHouse?.Name ?? "N/A"
            }).ToList();
            return result;
        }

        public async Task<RoomResponseDto> GetRoomByIdAsync(int id)
        {
            var phong = _context.Rooms.FirstOrDefault(p => p.RoomID == id);
            if (phong == null) return null;

            return new RoomResponseDto
            {
                RoomID = phong.RoomID,
                RoomNumber = phong.RoomNumber,
                Price = phong.Price,
                Status = phong.Status,
                CurrentTenants = phong.Customers?.Count ?? 0,
                MaxTenants = phong.Maxtenants,
                BoardingHouseName = phong.BoardingHouse?.Name ?? "N/A"
            };
        }

        public async Task<RoomResponseDto> CreateRoomAsync (RoomCreateDto createDto)
        {
            // Kiem tra Boarding House co ton tai khong?
            var boardinghouse = await _context.BoardingHouses
                .FirstOrDefaultAsync(bh => bh.BoardingHouseID == createDto.BoardingHouseID);
            if(boardinghouse == null)
                throw new Exception("Boarding House khong ton tai");

            // Kiem tra trung so phong trong cung mot day tro.
            bool IsRoomExist = await _context.Rooms.AnyAsync(r =>
                r.BoardingHouseID == createDto.BoardingHouseID &&
                r.RoomNumber == createDto.RoomNumber);

            if (IsRoomExist)
                throw new Exception("So Phong khong ton tai trong day tro!");

            // Map DTO sang Entity
            var room = _mapper.Map<Rooms>(createDto);

            // Thiet lap cac gia tri mac dinh neu Model chua co
            room.Status = 0;
            room.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            // Luu vao Database 
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            await _context.Entry(room).Reference(r => r.BoardingHouse).LoadAsync();

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task<RoomResponseDto> UpdateRoomAsync (RoomUpdateDto updateDto, int roomId)
        {
            // 1️⃣ Lấy OwnerId từ JWT
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // 2️⃣ Tìm Room + Include BoardingHouse để check quyền
            var room = await _context.Rooms
                .Include(r => r.BoardingHouse)
                .FirstOrDefaultAsync(r => 
                    r.RoomID == roomId );

            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            // 3️⃣ Check quyền (Owner của BoardingHouse)
            if (room.BoardingHouse?.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to update this room.");

            // 4️⃣ Update dữ liệu
            room.RoomNumber = updateDto.RoomNumber;
            room.Price = updateDto.Price;
            room.Maxtenants = updateDto.MaxTenants;

            await _context.SaveChangesAsync();

            // 5️⃣ Return DTO
            return new RoomResponseDto
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber,
                Price = room.Price,
                MaxTenants = room.Maxtenants,
            };
        }
        public async Task<RoomResponseDto>
    }
}