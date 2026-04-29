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
using QuanLyPhongTro.dtos.ServiceDTOs;
using System.Diagnostics;
using AutoMapper;

namespace QuanLyPhongTro.services.Implementations
{
    public class Service : IService
    {
        private readonly ManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Service (
            ManagementDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ServiceResponseDto>> GetAllServiceAsync()
        {
            var service = await _context.Services
                .Include(s => s.BoardingHouse)
                .Where(s => s.IsActive)
                .Select(s => new ServiceResponseDto
                {
                    ServiceID = s.ServiceID,
                    ServiceName = s.ServiceName,
                    Unit = s.Unit,
                    Price = s.Price,
                    IsActive = s.IsActive,
                    BoardingHouseName = s.BoardingHouse != null
                                        ? s.BoardingHouse.Name
                                        : string.Empty
                })
                .ToListAsync();
            return service;
        }

        public async Task<ServiceResponseDto?> GetServiceById(int id)
        {
            var service = await _context.Services
                .Include(s => s.BoardingHouse)
                .Where(s => s.ServiceID == id && s.IsActive)
                .Select(s => new ServiceResponseDto
                {
                   ServiceID = s.ServiceID,
                    ServiceName = s.ServiceName,
                    Unit = s.Unit,
                    Price = s.Price,
                    IsActive = s.IsActive,
                    BoardingHouseName = s.BoardingHouse != null
                                        ? s.BoardingHouse.Name
                                        : string.Empty
                }).FirstOrDefaultAsync();
            return service;    
        }

        public async Task<ServiceResponseDto> CreateService(ServiceCreateDto createDto)
        {
            // Kiểm tra BoardingHouse tồn tại
            var boardingHouse = await _context.BoardingHouses
                .FirstOrDefaultAsync(b => b.BoardingHouseID == createDto.BoardingHouseId);

            if (boardingHouse == null)
                throw new Exception("Boarding house not found");

            //  Tạo service mới
            var service = new Services
            {
                BoardingHouseID = createDto.BoardingHouseId,
                ServiceName = createDto.ServiceName,
                Price = createDto.Price,
                Unit = createDto.Unit,
                IsActive = true
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            //  Trả về DTO
            return new ServiceResponseDto
            {
                ServiceID = service.ServiceID,
                ServiceName = service.ServiceName,
                Unit = service.Unit,
                Price = service.Price,
                IsActive = service.IsActive,
                BoardingHouseName = boardingHouse.Name
            };
        }

        public async Task<ServiceResponseDto> UpdateService(ServiceUpdateDto updateDto, int id)
        {
            // 1️⃣ Tìm service
            var service = await _context.Services
                .Include(s => s.BoardingHouse)
                .FirstOrDefaultAsync(s => s.ServiceID == id);

            if (service == null)
                throw new Exception("Service not found");

            // 2️⃣ Validate dữ liệu
            if (updateDto.Price <= 0)
                throw new Exception("Price must be greater than zero");

            // 3️⃣ Kiểm tra trùng tên trong cùng BoardingHouse (nếu đổi tên)
            var isDuplicate = await _context.Services.AnyAsync(s =>
                s.ServiceID != id &&
                s.BoardingHouseID == service.BoardingHouseID &&
                s.ServiceName == updateDto.ServiceName &&
                s.IsActive);

            if (isDuplicate)
                throw new Exception("Service name already exists in this boarding house");

            // 4️⃣ Cập nhật dữ liệu
            service.ServiceName = updateDto.ServiceName.Trim();
            service.Unit = updateDto.Unit.Trim();
            service.Price = updateDto.Price;
            service.IsActive = updateDto.IsActive;

            await _context.SaveChangesAsync();

            // 5️⃣ Map sang DTO
            return new ServiceResponseDto
            {
                ServiceID = service.ServiceID,
                ServiceName = service.ServiceName,
                Unit = service.Unit,
                Price = service.Price,
                IsActive = service.IsActive,
                BoardingHouseName = service.BoardingHouse != null
                                        ? service.BoardingHouse.Name
                                        : string.Empty
            };
        }
    }
}