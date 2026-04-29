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
using QuanLyPhongTro.dtos.BillDTOs;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using QuanLyPhongTro.dtos.BillDetailDTOs;

namespace QuanLyPhongTro.services.Implementations
{
    public class BillDetailService : IBillDetailService
    {
        private readonly ManagementDbContext _context;
        public BillDetailService(ManagementDbContext context)
        {
            _context = context;
        }

        // ================= PRIVATE METHODS =================

        private async Task UpdateBillTotalAsync(int billId)
        {
            var bill = await _context.Bills
                .Include(b => b.BillDetails)
                .FirstOrDefaultAsync(b => b.BillID == billId);

            if (bill == null) return;

            bill.TotalAmount = bill.BillDetails.Sum(x => x.TotalPrice);

            await _context.SaveChangesAsync();
        }

        private static BillDetailResponseDto MapToResponse(BillDetails x)
        {
            return new BillDetailResponseDto
            {
                BillDetailID = x.BillDetailID,
                BillID = x.BillID,
                Description = x.Description,
                Quantity = x.Quantity,
                Unit = x.Unit,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.TotalPrice
            };
        }

        // Tạo mới chi tiết hóa đơn
        public async Task<BillDetailResponseDto> CreateAsync(BillDetailCreateDto createDto)
        {
            var bill = await _context.Bills
                .Include(b => b.BillDetails)
                .FirstOrDefaultAsync(b => b.BillID == createDto.BillID);
            if(bill == null) throw new Exception("Bill not found");
            var entity = new BillDetails
            {
                BillID = createDto.BillID,
                Description = createDto.Description,
                Quantity = createDto.Quantity,
                Unit = createDto.Unit,
                UnitPrice = createDto.UnitPrice,
                TotalPrice = createDto.Quantity * createDto.UnitPrice
            };
            _context.BillDetails.Add(entity);
            await _context.SaveChangesAsync();

            await UpdateBillTotalAsync(createDto.BillID);
            return MapToResponse(entity);
        }

        // Cập nhật chi tiết hóa đơn
        public async Task<BillDetailResponseDto?> UpdateAsync(int id, BillDetailUpdateDto updateDto)
        {
            var entity = await _context.BillDetails.FindAsync(id);

            if (entity == null) return null;

            entity.Description = updateDto.Description;
            entity.Quantity = updateDto.Quantity;
            entity.Unit = updateDto.Unit;
            entity.UnitPrice = updateDto.UnitPrice;
            entity.TotalPrice = updateDto.Quantity * updateDto.UnitPrice;

            await _context.SaveChangesAsync();

            // 🔥 update lại tổng bill
            await UpdateBillTotalAsync(entity.BillID);

            return MapToResponse(entity);
        }


        // Xóa chi tiết hóa đơn
        public async Task<bool> DeleteAsync(int id){
            var entity = await _context.BillDetails.FindAsync(id);
            if(entity == null) return false;
            int billId = entity.BillID;

            _context.BillDetails.Remove(entity);
            await _context.SaveChangesAsync();

            await UpdateBillTotalAsync(billId);
            return true;
        }

        // Lấy 1 chi tiết hóa đơn theo ID
        public async Task<BillDetailResponseDto?> GetByIdAsync(int id)
        {
            var entity = await _context.BillDetails.FindAsync(id);
            return entity == null ? null : MapToResponse(entity);
        }

        // Lấy tất cả chi tiết của 1 hóa đơn
        public async Task<IEnumerable<BillDetailResponseDto>> GetByBillIdAsync(int billId)
        {
            var list = await _context.BillDetails
                .Where(x => x.BillID == billId)
                .ToListAsync();
            return list.Select(MapToResponse);
        }

        // Lấy tất cả (ít dùng nhưng vẫn nên có cho admin)
        public async Task<IEnumerable<BillDetailResponseDto>> GetAllAsync()
        {
            var list = await _context.BillDetails.ToListAsync();
            return list.Select(MapToResponse);
        } 
    }
}