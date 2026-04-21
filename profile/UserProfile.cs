using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuanLyPhongTro.dtos.UserDTOs;

namespace QuanLyPhongTro.profile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // AuthResponseDto => entity
            CreateMap<AuthResponseDto, model.User>();
            // ChangePasswordDto => entity
            CreateMap<ChangePasswordDto, model.User>();
            //LoginDto => entity
            CreateMap<LoginDto, model.User>();
            // UserCreateDto => entity
            CreateMap<UserCreateDto, model.User>();
            // UserUpdateDto => entity
            CreateMap<UserUpdateDto, model.User>();
            // User => UserResponseDto
            CreateMap<model.User, UserResponseDto>();
        }

    }
}