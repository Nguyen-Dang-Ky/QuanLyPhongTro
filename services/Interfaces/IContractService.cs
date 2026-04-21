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
            Task<IEnumerable<ContractResponseDto>> GetAllContractsAsync();
            Task<ContractResponseDto?> GetContractByIdAsync(int id);
            Task<ContractResponseDto> CreateContractAsync(CreateContractDto createDto);
            Task<ContractResponseDto> UpdateContractAsync(int id, UpdateContractDto updateDto);
            Task<bool> DeleteContractAsync(int id);
        }
}
