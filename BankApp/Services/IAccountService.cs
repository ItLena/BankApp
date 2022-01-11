using BankApp.Models;

namespace BankApp.Services
{
    public interface IAccountService
    {
        List<Account> GetAccounts(string sortColumn, string sortOrder);

        Account ViewAccount(int accountId);
    }
}
