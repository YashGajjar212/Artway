using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Customers;
using Artway.DTOs.Customers;
using Artway.Models.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artway.Presentation.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;

        public CustomersController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
        {
            var result = await _customerServices.GetAllCustomers();

            if (result == null)
                return Ok(new List<Customer>());

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var result = await _customerServices.GetCustomerById(id);

            if (result == null)
                throw new NotFoundException(ExceptionMessages.CustomerNotFoundwithId(id));

            return Ok(result);
        }

        [HttpPost]
        // [Route("add")] This is not needed as per REST design. 
        public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerDto customer)
        {
            var newCustomer = await _customerServices.AddCustomer(customer);

            if (newCustomer == null)
            {
                throw new Exception(ExceptionMessages.CustomerNotFound); // Add a new exception
            }

            return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.CustomerId }, newCustomer);
        }

        [HttpPut]
        //[Route("update")] Onve again this is not needed as the URL will become api/customers/update and this is not the standard
        public async Task<ActionResult<CustomerDto>> UpdateCustomer([FromBody] CustomerDto customer)
        {
            var updatedCustomer = await _customerServices.UpdateCustomer(customer);

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerServices.DeleteCustomer(id);
            return NoContent();
        }
    }
}