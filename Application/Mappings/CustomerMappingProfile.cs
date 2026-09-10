using Artway.DTOs.Customers;
using Artway.Models;
using Artway.Models.Customers;
using AutoMapper;

namespace Artway.Application.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CustomerDto, Customer>().ReverseMap();
        }
    }
}