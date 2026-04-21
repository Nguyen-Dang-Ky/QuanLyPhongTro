using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos.BillDTOs;

namespace QuanLyPhongTro.profile
{
    public class BillProfile : Profile
    {
        public BillProfile()
        {
            // Create
            CreateMap<BillCreateDto, Bills>()
                .ForMember(dest => dest.TotalAmount,
                    opt => opt.MapFrom(src =>
                        src.RoomCharge + src.ServiceCharge + src.OtherCharges))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => BillStatus.Unpaid));

            // Update
            CreateMap<BillUpdateDto, Bills>()
                .ForMember(dest => dest.TotalAmount,
                    opt => opt.MapFrom(src =>
                        src.RoomCharge + src.ServiceCharge + src.OtherCharges));

            // Status Update
            CreateMap<BillStatusUpdateDto, Bills>();

            // Entity -> Response
            CreateMap<Bills, BillResponseDto>()
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null
                            ? src.Room.RoomNumber
                            : string.Empty));

            // Entity -> Detail
            CreateMap<Bills, BillDetailDto>()
                .ForMember(dest => dest.RoomNumber,
                    opt => opt.MapFrom(src =>
                        src.Room != null
                            ? src.Room.RoomNumber
                            : string.Empty))
                .ForMember(dest => dest.BillDetailCount,
                    opt => opt.MapFrom(src => src.BillDetails.Count))
                .ForMember(dest => dest.PaymentCount,
                    opt => opt.MapFrom(src => src.Payments.Count));

            // Simple
            CreateMap<Bills, BillSimpleDto>();
        }
    }
}