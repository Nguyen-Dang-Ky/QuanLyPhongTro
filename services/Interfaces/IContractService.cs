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
        Task<IEnumerable<ContractResponseDto>> GetAllContractAsync();
        Task<ContractResponseDto?> GetContractById(int id);
        Task<ContractResponseDto> createContractAsync(CreateContractDto createDto);
        Task<ContractResponseDto> updateContractAsync(UpdateContractDto updateDto);
        Task<bool> deleteContractAsync(int id);
    }
}
