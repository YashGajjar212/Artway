using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Customers;
using Artway.Models;

namespace Artway.Application.Services.Customers
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAllCustomers();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _customerRepository.GetCustomerById(id);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            var newCustomer = await _customerRepository.AddCustomer(customer);

            if (newCustomer == null)
                throw new Exception(ExceptionMessages.CustomerInsertException);

            return newCustomer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _customerRepository.GetCustomerById(customer.CustomerId);

            if (existingCustomer == null)
            {
                throw new NotFoundException($"Customer with ID {customer.CustomerId} was not found.");
            }

            existingCustomer.Name = customer.Name;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Email = customer.Email;
            existingCustomer.PasswordHash = customer.PasswordHash;
            existingCustomer.UserRole = customer.UserRole;
            existingCustomer.Creation_Date = customer.Creation_Date;
            existingCustomer.Last_Updated = DateTime.UtcNow;
            existingCustomer.Last_Login = customer.Last_Login;

            await _customerRepository.UpdateCustomer(existingCustomer);
            return existingCustomer;
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);

            if (customer == null)
                throw new NotFoundException($"Customer with ID {id} was not found.");

            await _customerRepository.DeleteCustomer(id);
        }
    }
}