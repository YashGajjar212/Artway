using Artway.Presentation.DTOs.Customers;

namespace Artway.Application.Interfaces.Customers
{
    public interface IAccountServices
    {
        public Task<List<AccountDto>> GetAllAccounts();

        public Task<AccountDto> GetAccountById(int id);
        
        public Task<AccountDto> GetAccountByEmail(string email);

        public Task<AccountDto> AddAccount(AccountDto accountDto);

        public Task<AccountDto> UpdateAccount(AccountDto accountDto);

        public Task DeleteAccount(int id);
    }
}