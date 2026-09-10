using Artway.DTOs.Customers;
using Artway.Models.Customers;

namespace Artway.Application.Interfaces.Customers
{
    public interface ICustomerServices
    {
        public Task<List<CustomerDto>> GetAllCustomers();

        public Task<CustomerDto> GetCustomerById(int id);
        
        public Task<CustomerDto> GetCustomerByEmail(string email);

        public Task<CustomerDto> AddCustomer(CustomerDto customer);

        public Task<CustomerDto> UpdateCustomer(CustomerDto customer);

        public Task DeleteCustomer(int id);
    }
}