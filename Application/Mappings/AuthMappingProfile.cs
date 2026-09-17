using Artway.Infrastructure.Models.Auth;
using Artway.Presentation.DTOs.Auth;
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
