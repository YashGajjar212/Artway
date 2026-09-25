using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Customers;
using Artway.Application.Services.Customers;
using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artway.Presentation.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountServices;

        public AccountController(IAccountService accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpGet("/api/accounts")]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccounts()
        {
            var result = await _accountServices.GetAllAccounts();

            if (result == null)
                return Ok(new List<Account>());

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(int id)
        {
            var result = await _accountServices.GetAccountById(id);

            if (result == null)
                throw new NotFoundException(ExceptionMessages.AccountNotFoundwithId(id));

            return Ok(result);
        }

        [HttpPost]
        // [Route("add")] This is not needed as per REST design. 
        public async Task<ActionResult<AccountDto>> AddAccount([FromBody] AccountDto account)
        {
            var newAccount = await _accountServices.AddAccount(account);

            if (newAccount == null)
            {
                throw new Exception(ExceptionMessages.AccountNotFound); // Add a new exception
            }

            return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.AccountId }, newAccount);
        }

        [HttpPut]
        //[Route("update")] Onve again this is not needed as the URL will become api/customers/update and this is not the standard
        public async Task<ActionResult<AccountDto>> UpdateAccount([FromBody] AccountDto account)
        {
            var updatedAccount = await _accountServices.UpdateAccount(account);

            return Ok(updatedAccount);
        }

        [HttpDelete("/api/accounts/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _accountServices.DeleteAccount(id);
            return NoContent();
        }
    }
}