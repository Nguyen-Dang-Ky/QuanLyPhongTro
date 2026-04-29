using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using QuanLyPhongTro.model;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.dtos.ServiceUsageDTOs;
using System.Diagnostics;
using AutoMapper;

namespace QuanLyPhongTro.services.Implementations
{
    public class UsageServices : IServiceUsages
    {
        private readonly ManagementDbContext _context;
        public UsageServices(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<SUResponseDto> CreateAsync(SUCreateDto dto)
        {
            // check trùng
            var exists = await _context.ServiceUsages.AnyAsync(x =>
                x.RoomID == dto.RoomID &&
                x.ServiceID == dto.ServiceID &&
                x.UsageMonth == dto.UsageMonth &&
                x.UsageYear == dto.UsageYear);

            if (exists)
                throw new Exception("Dữ liệu tháng này đã tồn tại");

            var service = await _context.Services.FindAsync(dto.ServiceID);
            if (service == null)
                throw new Exception("Service không tồn tại");

            var room = await _context.Rooms.FindAsync(dto.RoomID);
            if (room == null)
                throw new Exception("Room không tồn tại");

            var quantity = dto.NewIndex - dto.OldIndex;
            var totalPrice = quantity * service.Price;

            var entity = new ServiceUsages
            {
                ServiceID = dto.ServiceID,
                RoomID = dto.RoomID,
                UsageMonth = dto.UsageMonth,
                UsageYear = dto.UsageYear,
                OldIndex = dto.OldIndex,
                NewIndex = dto.NewIndex,
                Quantity = quantity.ToString(),
                TotalPrice = totalPrice
            };

            _context.ServiceUsages.Add(entity);
            await _context.SaveChangesAsync();

            return new SUResponseDto
            {
                ServiceUsageID = entity.ServiceUsageID,
                ServiceName = service.ServiceName,
                RoomNumber = room.RoomNumber,
                UsageMonth = entity.UsageMonth,
                UsageYear = entity.UsageYear,
                Quantity = entity.Quantity,
                TotalPrice = entity.TotalPrice
            };
        }

        public async Task<SUResponseDto?> UpdateAsync(SUUpdateDto dto, int id)
        {
            var entity = await _context.ServiceUsages.FindAsync(id);
            if (entity == null) return null;

            var service = await _context.Services.FindAsync(entity.ServiceID);
            if (service == null)
                throw new Exception("Service không tồn tại");

            var room = await _context.Rooms.FindAsync(entity.RoomID);
            if (room == null)
                throw new Exception("Room không tồn tại");

            var quantity = dto.NewIndex - dto.OldIndex;
            var totalPrice = quantity * service.Price;

            entity.OldIndex = dto.OldIndex;
            entity.NewIndex = dto.NewIndex;
            entity.Quantity = quantity.ToString();
            entity.TotalPrice = totalPrice;

            await _context.SaveChangesAsync();

            return new SUResponseDto
            {
                ServiceUsageID = entity.ServiceUsageID,
                ServiceName = service.ServiceName,
                RoomNumber = room.RoomNumber,
                UsageMonth = entity.UsageMonth,
                UsageYear = entity.UsageYear,
                Quantity = entity.Quantity,
                TotalPrice = entity.TotalPrice
            };
        }

        public async Task<SUResponseDto?> GetByRoomAndMonthAsync(int roomId, int month, int year)
        {
            var entity = await _context.ServiceUsages
                .Include(x => x.Service)
                .Include(x => x.Room)
                .FirstOrDefaultAsync(x =>
                    x.RoomID == roomId &&
                    x.UsageMonth == month &&
                    x.UsageYear == year);

            if (entity == null) return null;

            return new SUResponseDto
            {
                ServiceUsageID = entity.ServiceUsageID,
                ServiceName = entity.Service?.ServiceName ?? "",
                RoomNumber = entity.Room?.RoomNumber ?? "",
                UsageMonth = entity.UsageMonth,
                UsageYear = entity.UsageYear,
                Quantity = entity.Quantity,
                TotalPrice = entity.TotalPrice
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ServiceUsages.FindAsync(id);
            if (entity == null) return false;

            _context.ServiceUsages.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}