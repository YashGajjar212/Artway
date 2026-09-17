using Artway.Infrastructure.Models.Customers;

namespace Artway.Application.Interfaces.Customers
{
    public interface IAccountRepository
    {
        public Task<List<Account>> GetAllAccounts();

        public Task<Account> GetAccountById(int id);

        public Task<Account> GetAccountByEmail(string email);

        public Task<Account> AddAccount(Account account);

        public Task<Account> UpdateAccount(Account account);

        public Task DeleteAccount(int id);
    }
}