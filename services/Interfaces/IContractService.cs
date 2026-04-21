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
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomerAsync();
        Task<CustomerResponseDto?> GetCustomerById(int id);
        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createdto);
        Task<CustomerResponseDto> UpdateCustomerAsync(CustomerUpdateDto updatedto);
        Task<bool> DeleteCustomerAsync(int id); 

        Task<IEnumerable<ContractResponseDto>> GetAllContractAsync();
        Task<ContractResponseDto?> GetContractById(int id);
        Task<>
    }
}
