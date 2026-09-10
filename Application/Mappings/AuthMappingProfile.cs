using Artway.DTOs.Auth;
using Artway.Models.Auth;
using AutoMapper;

namespace Artway.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<LoginRequestDto, LoginRequest>().ReverseMap();
            CreateMap<LoginResponseDto, LoginResponse>().ReverseMap();
            CreateMap<RegisterRequestDto, RegisterRequest>().ReverseMap();
            CreateMap<RegisterResponseDto, RegisterResponse>().ReverseMap();
        }
    }
}
