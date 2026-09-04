using Artway.Application.Interfaces.Customers;
using Artway.Database.DBContext;
using Artway.Models;
using Microsoft.EntityFrameworkCore;

namespace Artway.Infrastructure.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ArtwayContext _context;
        public CustomerRepository(ArtwayContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await GetCustomerById(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}