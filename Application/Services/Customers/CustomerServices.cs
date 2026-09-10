using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Customers;
using Artway.DTOs.Customers;
using Artway.Models.Customers;
using AutoMapper;

namespace Artway.Application.Services.Customers
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerServices(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            var result = await _customerRepository.GetAllCustomers();
            var customer = _mapper.Map<List<CustomerDto>>(result);
            return customer;
        }

        public async Task<CustomerDto> GetCustomerById(int id)
        {
            var result = await _customerRepository.GetCustomerById(id);
            var customer = _mapper.Map<CustomerDto>(result);
            return customer;
        }

        public async Task<CustomerDto> GetCustomerByEmail(string email)
        {
            var result = await _customerRepository.GetCustomerByEmail(email);
            var customer = _mapper.Map<CustomerDto>(result);
            return customer;
        }

        public async Task<CustomerDto> AddCustomer(CustomerDto customer)
        {
            var custType = _mapper.Map<Customer>(customer);
            var result = await _customerRepository.AddCustomer(custType);

            if (result == null)
                throw new Exception(ExceptionMessages.CustomerInsertException);

            var addedCustomer = _mapper.Map<CustomerDto>(result);
            return addedCustomer;
        }

        public async Task<CustomerDto> UpdateCustomer(CustomerDto customer)
        {
            var existingCustomer = await _customerRepository.GetCustomerById(customer.CustomerId);

            if (existingCustomer == null)
            {
                throw new NotFoundException($"Customer with ID {customer.CustomerId} was not found.");
            }

            _mapper.Map(customer, existingCustomer);
            //existingCustomer.Name = customer.Name;
            //existingCustomer.Phone = customer.Phone;
            //existingCustomer.Email = customer.Email;
            //existingCustomer.PasswordHash = customer.PasswordHash;
            //existingCustomer.UserRole = customer.UserRole;
            //existingCustomer.Creation_Date = customer.Creation_Date;
            //existingCustomer.Last_Updated = DateTime.UtcNow;
            //existingCustomer.Last_Login = customer.Last_Login;

            await _customerRepository.UpdateCustomer(existingCustomer);

            return _mapper.Map<CustomerDto>(existingCustomer);
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