using BankApp.Models;

namespace BankApp.Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactions();
        int SaveNew(Transaction transaction);

    }
}
