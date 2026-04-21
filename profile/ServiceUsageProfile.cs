using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos.ServiceUsageDTOs;

namespace QuanLyPhongTro.profile
{
    public class ServiceUsageProfile : Profile
    {
        public ServiceUsageProfile()
        {
            // Create
            CreateMap<SUCreateDto, ServiceUsages>();

            // Update
            CreateMap<SUUpdateDto, ServiceUsages>();

            // Entity -> Response
            CreateMap<ServiceUsages, SUResponseDto>()
                .ForMember(dest => dest.ServiceName,
                    opt => opt.MapFrom(src =>
                        src.Service != null ? src.Service.ServiceName : string.Empty))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null ? src.Room.RoomNumber : string.Empty));

            // Entity -> Detail
            CreateMap<ServiceUsages, SUDetailDto>()
                .ForMember(dest => dest.ServiceName,
                    opt => opt.MapFrom(src =>
                        src.Service != null ? src.Service.ServiceName : string.Empty))
                .ForMember(dest => dest.ServicePrice,
                    opt => opt.MapFrom(src =>
                        src.Service != null ? src.Service.Price : 0))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null ? src.Room.RoomNumber : string.Empty));

            // Simple
            CreateMap<ServiceUsages, SUSimpleDto>();
        }
    }
}