using BankApp.Models;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankAppDataContext _context;

        public AccountService(BankAppDataContext context)
        {
            _context = context;
        }

       
        public List<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account ViewAccount(int accountId)
        {
            return _context.Accounts.First(e => e.AccountId == accountId);
        }

        public void Update(Account account)
        {
            _context.SaveChanges();
        }
    }
}
