using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.dtos.BoardingHouseDTOs;
using QuanLyPhongTro.dtos.UserDTOs.BoardingHouseDTOs;
using QuanLyPhongTro.model;


namespace QuanLyPhongTro.profile
{
    public class BoardingHouseProfile : Profile
    {
        public BoardingHouseProfile()
        {
            // Create
        CreateMap<BHCreateDto, BoardingHouse>();

        // Update
        CreateMap<BHUpdateDto, BoardingHouse>();

        // Entity -> Response
        CreateMap<BoardingHouse, BHResponseDto>()
            .ForMember(dest => dest.RoomCount,
                opt => opt.MapFrom(src => src.Rooms.Count));

        // Entity -> Detail
        CreateMap<BoardingHouse, BHDetailDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : string.Empty))
            .ForMember(dest => dest.RoomCount,
                opt => opt.MapFrom(src => src.Rooms.Count))
            .ForMember(dest => dest.ServiceCount,
                opt => opt.MapFrom(src => src.Services.Count));

        // Entity -> Simple
        CreateMap<BoardingHouse, BHSimpleDto>();
        }
    }
}