using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;


namespace BankApp.Pages.Account
{
    
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly BankAppDataContext _context;

        public DetailsModel(IAccountService accountService, ITransactionService transactionService, BankAppDataContext context)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _context = context;
        }

        //display info on header (page view)
        public int AccountId { get; set; }        
        public DateTime Created { get; set; }
        public decimal TotalBalance { get; set; }

        //transaktion list
        public class TransactionItem
        {
            public int TransactionId { get; set; }
            public int AccountId { get; set; }
            public DateTime Date { get; set; }
            public string Operation { get; set; } = null!;
            public decimal Amount { get; set; }
            public decimal CurrentBalance { get; set; }
            public string? Bank { get; set; }
            public string? ToAccount { get; set; }
            public string Comment { get; set; }
        }

        public List<TransactionItem> TransactionItems { get; set; }
        public int PageNr { get; set; }
        public int Size { get; set; }


        //public void OnGet(int accountId, int pageNr, int size)
        //{
        //    var account = _accountService.ViewAccount(accountId);
        //    AccountId = accountId;
        //    Created = account.Created;
        //    TotalBalance = account.Balance;
        //}

        public async Task OnGetAsync(int page, string jsonData, int accountId)
        {
            PageNr = page;
            List<TransactionItem> transactionItemsTemp;
            List<TransactionItem> tempTransactionItems;

            if (jsonData != null)
            {
                tempTransactionItems = JsonSerializer.Deserialize<List<TransactionItem>>(jsonData);
                TransactionItems = tempTransactionItems;
            }

            if (TransactionItems == null)
            {
                TransactionItems = _transactionService.GetTransactions()
                    .Where(t => t.AccountId == accountId)
                    .OrderByDescending(d => d.Date)
                    .Take(20)
                    .Select(t => new TransactionItem
                    {
                        Date = t.Date,
                        Operation = t.Operation,
                        Amount = t.Amount,
                        Bank = t.Bank,
                        ToAccount = t.Account,
                        Comment = t.Symbol,
                        CurrentBalance = t.Balance
                    }).ToList();
            }
            else
            {
                transactionItemsTemp = _transactionService.GetTransactions()
                   .Where(t => t.AccountId == accountId)
                   .OrderByDescending(d => d.Date)
                   .Take(20)
                   .Select(t => new TransactionItem
                   {
                       Date = t.Date,
                       Operation = t.Operation,
                       Amount = t.Amount,
                       Bank = t.Bank,
                       ToAccount = t.Account,
                       Comment = t.Symbol,
                       CurrentBalance = t.Balance
                   }).ToList();

                TransactionItems.AddRange(transactionItemsTemp);
            }
        }

    }

}

