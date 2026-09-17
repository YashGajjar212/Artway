using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;
using AutoMapper;

namespace Artway.Application.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountDto, Account>().ReverseMap();
        }
    }
}