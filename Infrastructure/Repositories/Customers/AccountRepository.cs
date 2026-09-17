using Artway.Application.Interfaces.Customers;
using Artway.Database.DBContext;
using Artway.Infrastructure.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace Artway.Infrastructure.Repositories.Customers
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ArtwayContext _context;
        public AccountRepository(ArtwayContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Account> AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task DeleteAccount(int id)
        {
            var account = await GetAccountById(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}