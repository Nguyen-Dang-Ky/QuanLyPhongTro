using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.RoomDTOs;

namespace QuanLyPhongTro.services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomResponseDto>> GetAllRoomAsync();
        Task<RoomResponseDto?> GetRoomByIdAsync(int id);
        Task<RoomResponseDto> CreateRoomAsync (RoomCreateDto createDto);
        Task<RoomResponseDto> UpdateRoomAsync (RoomUpdateDto updateDto, int roomId);
        Task<bool> DeleteRoomAsync (int id);
    }
}