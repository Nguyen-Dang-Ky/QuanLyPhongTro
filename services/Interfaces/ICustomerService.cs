using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.CustomerDTOs;
using QuanLyPhongTro.dtos;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomerAsync();
        Task<CustomerResponseDto?> GetCustomerById(int id);
        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto createdto);
        Task<CustomerResponseDto> UpdateCustomerAsync(CustomerUpdateDto updatedto, int id);
        Task<bool> DeleteCustomerAsync(int id); 
    }
}