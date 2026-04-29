using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.RoomDTOs;
using QuanLyPhongTro.dtos.ServiceDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IService
    {
        Task<IEnumerable<ServiceResponseDto>> GetAllServiceAsync();
        Task<ServiceResponseDto?> GetServiceById(int id);
        Task<ServiceResponseDto> CreateService(ServiceCreateDto createDto);
        Task<ServiceResponseDto> UpdateService(ServiceUpdateDto updateDto, int id);
    }
}