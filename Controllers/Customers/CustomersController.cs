using Artway.Application.Interfaces.Customers;
using Artway.Models;
using Microsoft.AspNetCore.Mvc;
using Artway.Application.Exceptions;

namespace Artway.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        public readonly ICustomerServices _customerServices;

        public CustomersController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            var result = await _customerServices.GetAllCustomers();

            if (!result.Any())
                throw new NotFoundException("No customers were found in the system.");

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var result = await _customerServices.GetCustomerById(id);

            if (result == null)
                throw new NotFoundException($"No customer was found with Id: {id}");

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task AddCustomer([FromBody]Customer customer)
        {
            await _customerServices.AddCustomer(customer);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<Customer>> UpdateCustomer([FromBody]Customer customer)
        {
            return await _customerServices.UpdateCustomer(customer);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task DeleteCustomer(int id)
        {
            await _customerServices.DeleteCustomer(id);
        }
    }
}