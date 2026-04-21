using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.dtos.PaymentDTOs;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.profile
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            // Create
            CreateMap<PaymentCreateDto, Payments>()
                .ForMember(dest => dest.PaymentID, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate,
                    opt => opt.MapFrom(src =>
                        src.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow)));

            // Update
            CreateMap<PaymentUpdateDto, Payments>()
                .ForMember(dest => dest.PaymentID, opt => opt.Ignore())
                .ForMember(dest => dest.BillID, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate,
                    opt => opt.MapFrom(src =>
                        src.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow)));

            // Entity -> Response
            CreateMap<Payments, PaymentResponseDto>();
        }
    }
}