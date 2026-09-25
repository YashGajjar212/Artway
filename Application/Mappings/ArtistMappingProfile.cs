using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;
using AutoMapper;

namespace Artway.Application.Mappings
{
    public class ArtistMappingProfile : Profile
    {
        public ArtistMappingProfile()
        {
            CreateMap<ArtistDto, Artist>().ReverseMap();
        }
    }
}