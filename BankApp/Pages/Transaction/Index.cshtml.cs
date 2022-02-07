using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages.Transaction
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public IndexModel(ITransactionService transactionService)
        {
           _transactionService=transactionService;
        }

        public class Item
        {
            public int TransactionId { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; }
            public string Operation { get; set; }
            public decimal Amount { get; set; }
            public  decimal Balance { get; set; }
            public int AccountId { get; set; }

        }
        public List<Item> Items { get; set; }
        public void OnGet()
        {
            var dateAndTime = DateTime.UtcNow;
            var inDate = dateAndTime.Date.AddDays(-1);
            Items = _transactionService.GetTransactions()
                     .Select(x => new Item
                     {
                         TransactionId = x.TransactionId,
                         Type = x.Type,
                         Operation = x.Operation,
                         AccountId = x.AccountId,
                         Amount = x.Amount,
                         Balance = x.Balance,
                         Date = x.Date
                     }).Where(c => c.Date >= inDate).OrderByDescending(x=>x.Date)
                     .ToList();
        }
    }
}

