using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.BillDTOs;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IBillService
    {
        // Tạo hóa đơn (thường dùng khi chốt tháng)
        Task<BillResponseDto> CreateBillAsync(BillCreateDto createDto);

        // Lấy tất cả hóa đơn (có thể filter sau)
        Task<IEnumerable<BillResponseDto>> GetAllBillsAsync();

        // Lấy theo ID
        Task<BillResponseDto?> GetBillByIdAsync(int id);

        // Lấy hóa đơn theo Room + tháng + năm
        Task<BillResponseDto?> GetBillByRoomAndMonthAsync(int roomId, int month, int year);

        // Cập nhật hóa đơn
        Task<BillResponseDto?> UpdateBillAsync(int id, BillUpdateDto updateDto);

        // Xóa hóa đơn
        Task<bool> CancelBillAsync(int id);



        // ===== Nghiệp vụ chính =====

        // Tính lại tổng tiền (Room + Service + Other)
        Task<bool> RecalculateTotalAsync(int billId);

        // Đánh dấu đã thanh toán
        Task<bool> MarkAsPaidAsync(int billId);

        // Đánh dấu quá hạn (có thể dùng background job)
        Task<bool> MarkAsOverdueAsync(int billId);

        // Tự động cập nhật trạng thái dựa vào DueDate
        Task<int> UpdateOverdueBillsAsync();



        // ===== Truy vấn nâng cao =====

        // Lấy hóa đơn theo phòng
        Task<IEnumerable<BillResponseDto>> GetBillsByRoomAsync(int roomId);

        // Lấy hóa đơn theo trạng thái
        Task<IEnumerable<BillResponseDto>> GetBillsByStatusAsync(BillStatus status);

        // Lấy hóa đơn theo tháng/năm
        Task<IEnumerable<BillResponseDto>> GetBillsByMonthAsync(int month, int year);
    }
}