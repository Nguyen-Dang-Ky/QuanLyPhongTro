using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.RoomDTOs;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using QuanLyPhongTro.model;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.dtos.BillDTOs;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace QuanLyPhongTro.services.Implementations
{
    public class BillService : IBillService
    {
        private readonly ManagementDbContext _context;
        public BillService (ManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BillResponseDto>> GetAllBillsAsync()
        {
            var bills = await _context.Bills
                .Include(b => b.Room)
                .Where(b => b.Status != BillStatus.Cancelled)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room != null ? b.Room.RoomNumber : string.Empty,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                })
                .ToListAsync();

            return bills;
        }

        public async Task<BillResponseDto?> GetBillByIdAsync(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Room)
                .Where(b => b.BillID == id && b.Status != BillStatus.Cancelled)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room != null ? b.Room.RoomNumber : string.Empty,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                })
                .FirstOrDefaultAsync();

            return bill;
        }

        public async Task<BillResponseDto?> GetBillByRoomAndMonthAsync(int roomId, int month, int year)
        {
            var bill = await _context.Bills
                .Include(b => b.Room)
                .Where(b => b.RoomID == roomId 
                        && b.Month == month 
                        && b.Year == year
                        && b.Status != BillStatus.Cancelled)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room != null ? b.Room.RoomNumber : string.Empty,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                })
                .FirstOrDefaultAsync();

            return bill;
        }

        public async Task<BillResponseDto> CreateBillAsync(BillCreateDto createDto)
        {
            // ❌ Check trùng hóa đơn
            var existingBill = await _context.Bills
                .FirstOrDefaultAsync(b => b.RoomID == createDto.RoomID
                                    && b.Month == createDto.Month
                                    && b.Year == createDto.Year
                                    && b.Status != BillStatus.Cancelled);

            if (existingBill != null)
                throw new Exception("Hóa đơn đã tồn tại cho phòng và tháng này");

            // ✔ Tạo bill
            var bill = new Bills
            {
                RoomID = createDto.RoomID,
                Month = createDto.Month,
                Year = createDto.Year,
                RoomCharge = createDto.RoomCharge,
                ServiceCharge = createDto.ServiceCharge,
                OtherCharges = createDto.OtherCharges,
                DueDate = createDto.DueDate,

                TotalAmount = createDto.RoomCharge 
                            + createDto.ServiceCharge 
                            + createDto.OtherCharges,

                Status = BillStatus.Unpaid,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            // Lấy RoomNumber
            var room = await _context.Rooms.FindAsync(bill.RoomID);

            return new BillResponseDto
            {
                BillID = bill.BillID,
                RoomNumber = room?.RoomNumber ?? string.Empty,
                Month = bill.Month,
                Year = bill.Year,
                TotalAmount = bill.TotalAmount,
                Status = bill.Status,
                DueDate = bill.DueDate
            };
        }

        public async Task<BillResponseDto?> UpdateBillAsync(int id, BillUpdateDto updateDto)
        {
            var bill = await _context.Bills
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BillID == id && b.Status != BillStatus.Cancelled);

            if (bill == null) return null;

            // ❌ Không cho sửa nếu đã thanh toán
            if (bill.Status == BillStatus.Paid)
                throw new Exception("Không thể cập nhật hóa đơn đã thanh toán");

            // ✔ Update
            bill.RoomCharge = updateDto.RoomCharge;
            bill.ServiceCharge = updateDto.ServiceCharge;
            bill.OtherCharges = updateDto.OtherCharges;
            bill.DueDate = updateDto.DueDate;

            // ✔ Tính lại tổng tiền
            bill.TotalAmount = bill.RoomCharge + bill.ServiceCharge + bill.OtherCharges;

            await _context.SaveChangesAsync();

            return new BillResponseDto
            {
                BillID = bill.BillID,
                RoomNumber = bill.Room?.RoomNumber ?? string.Empty,
                Month = bill.Month,
                Year = bill.Year,
                TotalAmount = bill.TotalAmount,
                Status = bill.Status,
                DueDate = bill.DueDate
            };
        }

        public async Task<bool> CancelBillAsync(int id)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null) return false;

            // ❌ Không cho hủy nếu đã thanh toán
            if (bill.Status == BillStatus.Paid)
                return false;

            // Nếu đã cancel rồi thì khỏi làm
            if (bill.Status == BillStatus.Cancelled)
                return false;

            bill.Status = BillStatus.Cancelled;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecalculateTotalAsync(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);

            if (bill == null || bill.Status == BillStatus.Cancelled)
                return false;

            bill.TotalAmount = bill.RoomCharge 
                            + bill.ServiceCharge 
                            + bill.OtherCharges;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsPaidAsync(int billId)
        {
            var bill = await _context.Bills
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BillID == billId);

            if (bill == null || bill.Status == BillStatus.Cancelled)
                return false;

            // Tổng tiền đã thanh toán
            var totalPaid = bill.Payments.Sum(p => p.AmountPaid);

            // ❌ Chưa trả đủ thì không cho Paid
            if (totalPaid < bill.TotalAmount)
                return false;

            bill.Status = BillStatus.Paid;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsOverdueAsync(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);

            if (bill == null || bill.Status == BillStatus.Cancelled)
                return false;

            // ❌ Không đụng vào bill đã thanh toán
            if (bill.Status == BillStatus.Paid)
                return false;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (bill.DueDate >= today)
                return false;

            bill.Status = BillStatus.Overdue;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateOverdueBillsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var bills = await _context.Bills
                .Where(b => b.Status != BillStatus.Paid
                        && b.Status != BillStatus.Cancelled
                        && b.DueDate < today)
                .ToListAsync();

            foreach (var bill in bills)
            {
                bill.Status = BillStatus.Overdue;
            }

            await _context.SaveChangesAsync();

            return bills.Count;
        }

        public async Task<IEnumerable<BillResponseDto>> GetBillsByRoomAsync(int roomId)
        {
            var bills = await _context.Bills
                .Where(b => b.RoomID == roomId && b.Status != BillStatus.Cancelled)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room!.RoomNumber,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                }).ToListAsync();
            return bills;
        }

        public async Task<IEnumerable<BillResponseDto>> GetBillsByStatusAsync(BillStatus status)
        {
            var bills = await _context.Bills
                .Where(b => b.Status == status)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room!.RoomNumber,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                }).ToListAsync();
            return bills;
        }

        public async Task<IEnumerable<BillResponseDto>> GetBillsByMonthAsync(int month, int year)
        {
            var bills = await _context.Bills
                .Where(b => b.Month == month 
                        && b.Year == year 
                        && b.Status != BillStatus.Cancelled)
                .Select(b => new BillResponseDto
                {
                    BillID = b.BillID,
                    RoomNumber = b.Room!.RoomNumber,
                    Month = b.Month,
                    Year = b.Year,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    DueDate = b.DueDate
                })
                .ToListAsync();

            return bills;
        }
    }
}