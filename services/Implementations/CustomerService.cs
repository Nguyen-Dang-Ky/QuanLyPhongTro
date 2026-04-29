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
using QuanLyPhongTro.dtos.CustomerDTOs;

namespace QuanLyPhongTro.services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerService (ManagementDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // =============
        // Get All Customer
        // =============
        public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomerAsync()
        {
            var customer = await _context.Customers
                .Include(c => c.Rooms)
                .Include(c => c.User)
                .ToListAsync();

            return customer.Select(c => new CustomerResponseDto
            {
                CustomerID = c.CustomerID,
                UserID = c.UserID,
                CustomerName = c.User?.FullName ?? "N/A",
                RoomID = c.RoomID,
                RoomNumber = c.Rooms?.RoomNumber ?? "Chua co phong",
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                IsActive = c.IsActive
            });
        }

        // =============
        // Get All Customer by ID
        // =============
        public async Task<CustomerResponseDto?> GetCustomerById(int id)
        {
            var cus = await _context.Customers
                .Include(c => c.Rooms)
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            if(cus == null) 
            return null;

            return new CustomerResponseDto
            {
                CustomerID = cus.CustomerID,
                UserID = cus.UserID,
                RoomID = cus.RoomID,
                RoomNumber = cus.Rooms?.RoomNumber ?? "N/A",
                StartDate = cus.StartDate,
                EndDate = cus.EndDate,
                IsActive = cus.IsActive
            };
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createDto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            // 1️⃣ Kiểm tra Room tồn tại + thuộc Owner
            var room = await _context.Rooms
                .Include(r => r.BoardingHouse)
                .FirstOrDefaultAsync(r => r.RoomID == createDto.RoomID);

            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            if (room?.BoardingHouse?.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to add customer to this room.");

            // 2️⃣ Không cho 2 customer active trong cùng 1 room
            bool hasActiveCustomer = await _context.Customers
                .AnyAsync(c => c.RoomID == createDto.RoomID && c.IsActive);

            if (hasActiveCustomer)
                throw new Exception("Room already has active customer.");

            // 3️⃣ Check trùng CMND/CCCD
            bool identityExists = await _context.Customers
                .AnyAsync(c => c.IdentityNumber == createDto.IdentityNumber && c.IsActive);

            if (identityExists)
                throw new Exception("Identity number already exists.");

            var customer = new Customers
            {
                UserID = createDto.UserID,
                RoomID = createDto.RoomID,
                IdentityNumber = createDto.IdentityNumber,
                PermanentAddress = createDto.PermanentAddress,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerResponseDto
            {
                CustomerID = customer.CustomerID,
                UserID = customer.UserID,
                RoomID = customer.RoomID,
                CustomerName = customer.User?.FullName ?? "N/A",
                IdentityNumber = customer.IdentityNumber,
                PermanentAddress = customer.PermanentAddress,
                StartDate = customer.StartDate,
                EndDate = customer.EndDate,
                IsActive = customer.IsActive
            };

        }
        public async Task<CustomerResponseDto> UpdateCustomerAsync(CustomerUpdateDto updateDto, int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Rooms!)
                    .ThenInclude(r => r.BoardingHouse)
                .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsActive);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            if (customer?.Rooms?.BoardingHouse?.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to update this customer.");

            customer.IdentityNumber = updateDto.IdentityNumber;
            customer.PermanentAddress = updateDto.PermanentAddress;
            customer.StartDate = updateDto.StartDate;
            customer.EndDate = updateDto.EndDate;

            await _context.SaveChangesAsync();

            return new CustomerResponseDto
            {
                CustomerID = customer.CustomerID,
                UserID = customer.UserID,
                RoomID = customer.RoomID,
                IdentityNumber = customer.IdentityNumber,
                PermanentAddress = customer.PermanentAddress,
                StartDate = customer.StartDate,
                EndDate = customer.EndDate,
                IsActive = customer.IsActive
            };
        }
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Rooms!)
                    .ThenInclude(r => r.BoardingHouse)
                .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsActive);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated.");

            int ownerId = int.Parse(userIdClaim);

            if (customer?.Rooms?.BoardingHouse?.OwnerID != ownerId)
                throw new UnauthorizedAccessException("You are not allowed to delete this customer.");

            customer.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}