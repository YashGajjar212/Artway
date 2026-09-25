using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Customers;
using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;
using AutoMapper;

namespace Artway.Application.Services.Customers
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<List<AccountDto>> GetAllAccounts()
        {
            var result = await _accountRepository.GetAllAccounts();
            var account = _mapper.Map<List<AccountDto>>(result);
            return account;
        }

        public async Task<AccountDto> GetAccountById(int id)
        {
            var result = await _accountRepository.GetAccountById(id);
            var account = _mapper.Map<AccountDto>(result);
            return account;
        }

        public async Task<AccountDto> GetAccountByEmail(string email)
        {
            var result = await _accountRepository.GetAccountByEmail(email);
            var account = _mapper.Map<AccountDto>(result);
            return account;
        }

        public async Task<AccountDto> AddAccount(AccountDto account)
        {
            var accType = _mapper.Map<Account>(account);
            var result = await _accountRepository.AddAccount(accType);

            if (result == null)
                throw new Exception(ExceptionMessages.AccountInsertException);

            var addedAccount = _mapper.Map<AccountDto>(result);
            return addedAccount;
        }

        public async Task<AccountDto> UpdateAccount(AccountDto account)
        {
            var existingAccount = await _accountRepository.GetAccountById(account.AccountId);

            if (existingAccount == null)
            {
                throw new NotFoundException("Account not found");
            }

            _mapper.Map(account, existingAccount);
            //existingCustomer.Name = customer.Name;
            //existingCustomer.Phone = customer.Phone;
            //existingCustomer.Email = customer.Email;
            //existingCustomer.PasswordHash = customer.PasswordHash;
            //existingCustomer.UserRole = customer.UserRole;
            //existingCustomer.Creation_Date = customer.Creation_Date;
            //existingCustomer.Last_Updated = DateTime.UtcNow;
            //existingCustomer.Last_Login = customer.Last_Login;

            await _accountRepository.UpdateAccount(existingAccount);

            return _mapper.Map<AccountDto>(existingAccount);
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _accountRepository.GetAccountById(id);

            if (account == null)
                throw new NotFoundException("Customer not found");

            await _accountRepository.DeleteAccount(id);
        }
    }
}