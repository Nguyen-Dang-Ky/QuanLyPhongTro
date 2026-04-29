using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLyPhongTro.dtos.ServiceUsageDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IServiceUsages
    {
        // Tạo mới (Nhập chỉ số tháng)
        Task<SUResponseDto> CreateAsync(SUCreateDto dto);

        // Cập nhật chỉ số
        Task<SUResponseDto?> UpdateAsync(SUUpdateDto dto, int id);

        //Lấy theo phòng + tháng + năm
        Task<SUResponseDto?> GetByRoomAndMonthAsync(int roomId, int month, int year);

        // Xóa 
        Task<bool> DeleteAsync(int id);
    }
}