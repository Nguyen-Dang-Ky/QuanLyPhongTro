using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.PaymentDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IPaymentService
    {
        // Tạo thanh toán mới
        Task<PaymentResponseDto> CreateAsync(PaymentCreateDto createDto);

        // Cập nhật thanh toán (ít dùng nhưng vẫn cần)
        Task<PaymentResponseDto?> UpdateAsync(int id, PaymentUpdateDto updateDto);

        // Xóa thanh toán
        Task<bool> DeleteAsync(int id);

        // Lấy theo ID
        Task<PaymentResponseDto?> GetByIdAsync(int id);

        // Lấy tất cả payment của 1 bill
        Task<IEnumerable<PaymentResponseDto>> GetByBillIdAsync(int billId);

        // Lấy tất cả
        Task<IEnumerable<PaymentResponseDto>> GetAllAsync();
    }
}