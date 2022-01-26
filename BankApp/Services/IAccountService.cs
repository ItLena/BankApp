using BankApp.Models;

namespace BankApp.Services
{
    public interface IAccountService
    {
        List<Account> GetAllAccounts();

        Account ViewAccount(int accountId);
        void Update(Account account);
        int SaveNew(Account account);
    }
}
