using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.BoardingHouseDTOs;
using QuanLyPhongTro.dtos;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IBoardingHouseService
    {
        Task<IEnumerable<BHResponseDto>> GetBoardingHousesAsync();
        Task<BHResponseDto?> GetBoardingHouseByIdAsync(int id);
        Task<BHResponseDto> CreateBoardingHouseAsync(BHCreateDto createDto);
        Task<BHResponseDto> UpdateBoardingHouseAsync(int id, BHUpdateDto updateDto);
        Task<bool> DeleteBoardingHouseAsync(int id);
    }
}