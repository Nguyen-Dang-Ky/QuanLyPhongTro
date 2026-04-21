using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.model;
using QuanLyPhongTro.dtos.BillDetailDTOs;

namespace QuanLyPhongTro.profile
{
    public class BillDetailProfile : Profile
    {
        public BillDetailProfile()
        {
            // Create
            CreateMap<BillDetailCreateDto, BillDetails>()
                .ForMember(dest => dest.TotalPrice,
                        opt => opt.MapFrom(src => src.UnitPrice)); 
            // Nếu bạn muốn tính TotalPrice phức tạp hơn,
            // nên tính trong Service thay vì AutoMapper

            // Update
            CreateMap<BillDetailUpdateDto, BillDetails>()
                .ForMember(dest => dest.BillDetailID, opt => opt.Ignore())
                .ForMember(dest => dest.BillID, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice,
                        opt => opt.MapFrom(src => src.UnitPrice));

            // Entity -> Response
            CreateMap<BillDetails, BillDetailResponseDto>();
        }
    }
}