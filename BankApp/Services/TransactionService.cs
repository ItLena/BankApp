using BankApp.Models;

namespace BankApp.Services
{
    public class TransactionService:ITransactionService
    {
        private readonly BankAppDataContext _context;

        public TransactionService(BankAppDataContext context)
        {
            _context = context;
        }
        
        IEnumerable<Transaction> ITransactionService.GetTransactions()
        {
            return _context.Transactions.OrderByDescending(d => d.Date).ToList();
        }
    }
}
