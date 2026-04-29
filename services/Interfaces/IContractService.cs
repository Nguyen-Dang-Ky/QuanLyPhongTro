using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.ContractDTOs;
using QuanLyPhongTro.dtos;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IContractService
        {
            // Tạo hợp đồng
            Task<ContractResponseDto> CreateContractAsync(ContractCreateDto createDto);

            // Lấy tất cả hợp đồng (có thể filter)
            Task<IEnumerable<ContractResponseDto>> GetAllContractsAsync();

            // Lấy theo ID
            Task<ContractResponseDto?> GetContractByIdAsync(int id);

            // Lấy theo Room
            Task<IEnumerable<ContractResponseDto>> GetContractsByRoomAsync(int roomId);

            // Lấy theo Customer
            Task<IEnumerable<ContractResponseDto>> GetContractsByCustomerAsync(int customerId);

            // Cập nhật hợp đồng
            Task<ContractResponseDto?> UpdateContractAsync(int id, ContractUpdateDto updateDto);

            // Hủy hợp đồng
            Task<bool> CancelContractAsync(int id);

            // Đánh dấu hết hạn (có thể dùng cho background job)
            Task<bool> MarkAsExpiredAsync(int id);
        }
}
