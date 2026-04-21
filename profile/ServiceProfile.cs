using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos.ServiceDTOs;

namespace QuanLyPhongTro.profile
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            // Create
            CreateMap<ServiceCreateDto, Services>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => true));

            // Update
            CreateMap<ServiceUpdateDto, Services>();

            // Entity -> Response
            CreateMap<Services, ServiceResponseDto>()
                .ForMember(dest => dest.BoardingHouseName,
                    opt => opt.MapFrom(src =>
                        src.BoardingHouse != null
                            ? src.BoardingHouse.Name
                            : string.Empty));

            // Entity -> Detail
            CreateMap<Services, ServiceDetailDto>()
                .ForMember(dest => dest.BoardingHouseName,
                    opt => opt.MapFrom(src =>
                        src.BoardingHouse != null
                            ? src.BoardingHouse.Name
                            : string.Empty))
                .ForMember(dest => dest.UsageCount,
                    opt => opt.MapFrom(src => src.ServiceUsages.Count));

            // Simple
            CreateMap<Services, ServiceSimpleDto>();
        }
    }
}