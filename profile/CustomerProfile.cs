using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.dtos.CustomerDTOs;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.profile
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Create
            CreateMap<CustomerCreateDto, Customers>();

            // Update
            CreateMap<CustomerUpdateDto, Customers>();

            // Entity -> Response
            CreateMap<Customers, CustomerResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src => src.Rooms != null ? src.Rooms.RoomNumber : string.Empty));

            // Entity -> Detail
            CreateMap<Customers, CustomerDetailDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : string.Empty))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src => src.Rooms != null ? src.Rooms.RoomNumber : string.Empty))
                .ForMember(dest => dest.ContractCount,
                    opt => opt.MapFrom(src => src.Contracts.Count));

            // Simple
            CreateMap<Customers, CustomerSimpleDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));
        }
    }
}