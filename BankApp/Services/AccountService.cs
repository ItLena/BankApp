using BankApp.Models;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankAppDataContext _accountContext;

        public AccountService(BankAppDataContext accountContext)
        {
            _accountContext = accountContext;
        }

       
        public List<Account> GetAccounts(string sortColumn, string sortOrder)
        {
            var query = _accountContext.Accounts.AsQueryable();

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(r => r.AccountId);
                else
                    query = query.OrderByDescending(r => r.AccountId);
            return query.ToList();
        }

        public Account ViewAccount(int accountId)
        {
            return _accountContext.Accounts.First(e => e.AccountId == accountId);
        }
    }
}
