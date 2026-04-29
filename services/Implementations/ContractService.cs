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
using QuanLyPhongTro.dtos.ContractDTOs;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace QuanLyPhongTro.services.Implementations
{
    public class ContractService : IContractService
    {
        private readonly ManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContractService (ManagementDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ContractResponseDto> CreateContractAsync(ContractCreateDto createDto)
        {
            // 1. Validate Customer
            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerID == createDto.CustomerID);

            if (customer == null)
                throw new Exception("Customer không tồn tại");

            // 2. Validate Room
            var room = await _context.Rooms
                .Include(r => r.Contracts)
                .FirstOrDefaultAsync(r => r.RoomID == createDto.RoomID);

            if (room == null)
                throw new Exception("Room không tồn tại");

            // 3. Check contract active
            var hasActiveContract = room.Contracts
                .Any(c => c.Status == ContractStatus.Active);

            if (hasActiveContract)
                throw new Exception("Phòng đã có hợp đồng đang hoạt động");

            // 4. Validate ngày
            if (createDto.EndDate <= createDto.StartDate)
                throw new Exception("EndDate phải lớn hơn StartDate");

            // 5. Tạo contract
            var contract = new Contracts
            {
                CustomerID = createDto.CustomerID,
                RoomID = createDto.RoomID,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                DepositAmount = createDto.DepositAmount,
                MonthlyPrice = createDto.MonthlyPrice,
                Status = ContractStatus.Active,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // 6. Save DB
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            // ❗ IMPORTANT: dùng dữ liệu đã load sẵn (customer, room), KHÔNG dùng contract.Customer

            var response = new ContractResponseDto
            {
                ContractID = contract.ContractID,

                CustomerName = customer.User?.FullName ?? string.Empty,

                RoomNumber = room.RoomNumber ?? string.Empty,

                StartDate = contract.StartDate,
                EndDate = contract.EndDate,

                MonthlyPrice = contract.MonthlyPrice,

                Status = contract.Status
            };

            return response;
        }

        public async Task<IEnumerable<ContractResponseDto>> GetAllContractsAsync()
        {
            var contract = await _context.Contracts
                .Include(c => c.Customer)
                .Include(c => c.Room)
                .ToListAsync();
            var result = contract.Select(c => new ContractResponseDto
            {
                ContractID = c.ContractID,
                CustomerName = c.Customer?.User?.FullName ?? "N/A",
                RoomNumber = c.Room?.RoomNumber ?? "N/A",
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MonthlyPrice = c.MonthlyPrice,
                Status = c.Status
            }).ToList();
            return result;
        }

        public async Task<ContractResponseDto?> GetContractByIdAsync(int id)
        {
            var contract = await _context.Contracts
                .Where(c => c.ContractID == id)
                .Select(c => new ContractResponseDto
                {
                    ContractID = c.ContractID,

                    CustomerName = c.Customer != null && c.Customer.User != null
                        ? c.Customer.User.FullName
                        : string.Empty,

                    RoomNumber = c.Room != null
                        ? c.Room.RoomNumber
                        : string.Empty,

                    StartDate = c.StartDate,
                    EndDate = c.EndDate,

                    MonthlyPrice = c.MonthlyPrice,

                    Status = c.Status
                })
                .FirstOrDefaultAsync();

            return contract;
        }

        public async Task<IEnumerable<ContractResponseDto>> GetContractsByRoomAsync(int roomId)
        {
            var contracts = await _context.Contracts
                .Where(c => c.RoomID == roomId)
                .Select(c => new ContractResponseDto
                {
                    ContractID = c.ContractID,

                    CustomerName = c.Customer != null && c.Customer.User != null
                        ? c.Customer.User.FullName
                        : string.Empty,

                    RoomNumber = c.Room != null
                        ? c.Room.RoomNumber
                        : string.Empty,

                    StartDate = c.StartDate,
                    EndDate = c.EndDate,

                    MonthlyPrice = c.MonthlyPrice,

                    Status = c.Status
                })
                .ToListAsync();

            return contracts;
        }

        public async Task<IEnumerable<ContractResponseDto>> GetContractsByCustomerAsync(int customerId)
        {
            var contracts = await _context.Contracts
                .Where(c => c.CustomerID == customerId)
                .Select(c => new ContractResponseDto
                {
                    ContractID = c.ContractID,

                    CustomerName = c.Customer != null && c.Customer.User != null
                        ? c.Customer.User.FullName
                        : string.Empty,

                    RoomNumber = c.Room != null
                        ? c.Room.RoomNumber
                        : string.Empty,

                    StartDate = c.StartDate,
                    EndDate = c.EndDate,

                    MonthlyPrice = c.MonthlyPrice,

                    Status = c.Status
                })
                .ToListAsync();

            return contracts;
        }

        public async Task<ContractResponseDto?> UpdateContractAsync(int id, ContractUpdateDto updateDto)
        {
            var contract = await _context.Contracts
                .Include(c => c.Customer!)
                    .ThenInclude(c => c.User)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(c => c.ContractID == id);

            if (contract == null)
                return null;

            // ❗ Không cho update nếu đã Cancelled
            if (contract.Status == ContractStatus.Cancelled)
                throw new Exception("Không thể cập nhật hợp đồng đã hủy");

            // Validate ngày
            if (updateDto.EndDate <= updateDto.StartDate)
                throw new Exception("EndDate phải lớn hơn StartDate");

            // Update dữ liệu
            contract.StartDate = updateDto.StartDate;
            contract.EndDate = updateDto.EndDate;
            contract.DepositAmount = updateDto.DepositAmount;
            contract.MonthlyPrice = updateDto.MonthlyPrice;
            contract.Status = updateDto.Status;

            await _context.SaveChangesAsync();

            // Map DTO
            return new ContractResponseDto
            {
                ContractID = contract.ContractID,

                CustomerName = contract.Customer?.User?.FullName ?? string.Empty,
                RoomNumber = contract.Room?.RoomNumber ?? string.Empty,

                StartDate = contract.StartDate,
                EndDate = contract.EndDate,

                MonthlyPrice = contract.MonthlyPrice,

                Status = contract.Status
            };
        }

        public async Task<bool> CancelContractAsync(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Room)
                .FirstOrDefaultAsync(c => c.ContractID == id);

            if (contract == null)
                return false;

            // Nếu đã cancel rồi thì thôi
            if (contract.Status == ContractStatus.Cancelled)
                return false;

            contract.Status = ContractStatus.Cancelled;

            // 👉 Nếu bạn có Room.Status thì update ở đây
            // contract.Room.Status = RoomStatus.Available;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAsExpiredAsync(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Room)
                .FirstOrDefaultAsync(c => c.ContractID == id);

            if (contract == null)
                return false;

            // Chỉ cho expire khi đang active
            if (contract.Status != ContractStatus.Active)
                return false;

            contract.Status = ContractStatus.Expired;

            // 👉 Nếu có Room.Status thì set lại phòng trống
            // contract.Room.Status = RoomStatus.Available;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}