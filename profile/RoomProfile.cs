
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos.RoomDTOs;

namespace QuanLyPhongTro.profile
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            // Create
            CreateMap<RoomCreateDto, Rooms>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => RoomStatus.Available));

            // Update
            CreateMap<RoomUpdateDto, Rooms>();

            // Status Update
            CreateMap<RoomStatusUpdateDto, Rooms>();

            // Entity -> Response
            CreateMap<Rooms, RoomResponseDto>()
                .ForMember(dest => dest.BoardingHouseName,
                    opt => opt.MapFrom(src =>
                        src.BoardingHouse != null
                            ? src.BoardingHouse.Name
                            : string.Empty))
                .ForMember(dest => dest.CurrentTenants,
                    opt => opt.MapFrom(src =>
                        src.Customers.Count(c => c.IsActive)));

            // Entity -> Detail
            CreateMap<Rooms, RoomDetailDto>()
                .ForMember(dest => dest.BoardingHouseName,
                    opt => opt.MapFrom(src =>
                        src.BoardingHouse != null
                            ? src.BoardingHouse.Name
                            : string.Empty))
                .ForMember(dest => dest.CurrentTenants,
                    opt => opt.MapFrom(src =>
                        src.Customers.Count(c => c.IsActive)))
                .ForMember(dest => dest.ContractCount,
                    opt => opt.MapFrom(src => src.Contracts.Count))
                .ForMember(dest => dest.BillCount,
                    opt => opt.MapFrom(src => src.Bills.Count));

            // Simple
            CreateMap<Rooms, RoomSimpleDto>();
        }
    }
}