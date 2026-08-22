using Artway.Models;

namespace Artway.Application.Interfaces.Customers
{
    public interface ICustomerServices
    {
        public Task<List<Customer>> GetAllCustomers();

        public Task<Customer> GetCustomerById(int id);

        public Task AddCustomer(Customer customer);

        public Task<Customer> UpdateCustomer(Customer customer);

        public Task DeleteCustomer(int id);
    }
}