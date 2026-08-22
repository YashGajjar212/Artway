using Artway.Application.Interfaces.Customers;
using Artway.Database.DBContext;
using Artway.Models;
using Microsoft.EntityFrameworkCore;

namespace Artway.Application.Services.Customers
{
    public class CustomerServices : ICustomerServices
    {
        public readonly ArtwayContext _context;
        public CustomerServices(ArtwayContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
        }

        public async Task AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
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
            _context.SaveChangesAsync();
        }
    }
}