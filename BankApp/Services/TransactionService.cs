using BankApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Services
{
    public class TransactionService:ITransactionService
    {
        private readonly BankAppDataContext _context;

        public TransactionService(BankAppDataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        IEnumerable<Transaction> ITransactionService.GetTransactions()
        {
            return _context.Transactions.ToList();
        }

       public int SaveNew(Transaction transaction)
        {
            _context.Transactions.Add(transaction);

            _context.SaveChanges();
            return transaction.TransactionId;
        }
    }
}
