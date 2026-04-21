using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.dtos.ContractDTOs;

namespace QuanLyPhongTro.profile
{
    public class ContractProfile : Profile
    {
        public ContractProfile()
        {
            // Create
            CreateMap<ContractCreateDto, Contracts>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ContractStatus.Active));

            // Update
            CreateMap<ContractUpdateDto, Contracts>();

            // Entity -> Response
            CreateMap<Contracts, ContractResponseDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.Customer != null && src.Customer.User != null
                            ? src.Customer.User.FullName
                            : string.Empty))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null ? src.Room.RoomNumber : string.Empty));

            // Entity -> Detail
            CreateMap<Contracts, ContractDetailDto>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.Customer != null && src.Customer.User != null
                            ? src.Customer.User.FullName
                            : string.Empty))
                .ForMember(dest => dest.CustomerPhone,
                    opt => opt.MapFrom(src =>
                        src.Customer != null && src.Customer.User != null
                            ? src.Customer.User.PhoneNumber
                            : string.Empty))
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null ? src.Room.RoomNumber : string.Empty));

            // Simple
            CreateMap<Contracts, ContractSimpleDto>();
        }
    }
}