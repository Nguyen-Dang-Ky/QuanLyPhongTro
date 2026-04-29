using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.BillDetailDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IBillDetailService
    {
        // Tạo mới chi tiết hóa đơn
        Task<BillDetailResponseDto> CreateAsync(BillDetailCreateDto createDto);

        // Cập nhật chi tiết hóa đơn
        Task<BillDetailResponseDto?> UpdateAsync(int id, BillDetailUpdateDto updateDto);

        // Xóa chi tiết hóa đơn
        Task<bool> DeleteAsync(int id);

        // Lấy 1 chi tiết hóa đơn theo ID
        Task<BillDetailResponseDto?> GetByIdAsync(int id);

        // Lấy tất cả chi tiết của 1 hóa đơn
        Task<IEnumerable<BillDetailResponseDto>> GetByBillIdAsync(int billId);

        // Lấy tất cả (ít dùng nhưng vẫn nên có cho admin)
        Task<IEnumerable<BillDetailResponseDto>> GetAllAsync();
    }
}