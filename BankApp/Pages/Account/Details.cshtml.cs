using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public DetailsModel(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }


        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }


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
        public string SearchPhrase { get; set; }

        public void OnGet(int accountId, string searchPhrase)
        {
            var account = _accountService.ViewAccount(accountId);
            AccountId = account.AccountId;
            Created = account.Created;
            TotalBalance = account.Balance;

            SearchPhrase = searchPhrase;

            if (searchPhrase != null)
            {

                TransactionItems = _transactionService.GetTransactions()
                    .Select(t => new TransactionItem
                    {
                        Date = t.Date,
                        Operation = t.Operation,
                        Amount = t.Amount,
                        Bank = t.Bank,
                        ToAccount =t.Account,
                        Comment = t.Symbol,
                        CurrentBalance = t.Balance
                    }).Where(x => x.Comment.ToLower().Contains(searchPhrase.ToLower()) || x.Bank.ToLower().Contains(searchPhrase.ToLower())).ToList();
            }

            else
            {
                TransactionItems = _transactionService.GetTransactions()
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

        }
    }

}
