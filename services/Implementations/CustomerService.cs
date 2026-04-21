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
        public CustomerService (ManagementDbContext context)
        {
            _context = context;
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

        public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createDto){}
        public async Task<CustomerResponseDto> UpdateCustomerAsync(CustomerUpdateDto updateDto){}
        public async Task<bool> DeleteCustomerAsync(int id){}
    }
}