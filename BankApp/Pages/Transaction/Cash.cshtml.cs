using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Transaction
{
    public enum Types
    {
        Credit,
        Debit
    }
    
    [BindProperties]
    public class CashModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public CashModel(ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("Choose type of transaction"), Required, StringLength(10)]
        public string Type { get; set; } = null!;

        [DisplayName("Choose type of operation"), Required, StringLength(50)]
        public string Operation { get; set; } = null!;

        [DisplayName("Amount to transact"), Required, Range(10, 10000)]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        [DisplayName("Comment"), StringLength(50)]
        public string? Comment { get; set; }

        public void OnGet(int accountId)
        {
            var account = _accountService.ViewAccount(accountId);
            AccountId = accountId;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountId, string operation)
        {
            var accountSaldo = _accountService.ViewAccount(accountId);
            Balance = accountSaldo.Balance;
            Operation = operation.Replace("_", " ");

            if (ModelState.IsValid)
            {
                if (Type == "Credit")
                {
                    Balance = Balance + Amount;
                }
                else
                {
                    if (Amount > Balance)
                    {
                        ModelState.AddModelError("Amount", "Amount is more than balance");
                        return Page();
                    }
                    else
                    {
                        Balance = Balance - Amount;
                        Amount *= -1;
                    }
                }
                var transaction = new Models.Transaction
                {
                    Operation = Operation,
                    Type = Type,
                    Amount = Amount,
                    Date = DateTime.UtcNow,
                    Balance = Balance,
                    Symbol = Comment,
                    AccountId = accountId,
                };
                int transactionId = _transactionService.SaveNew(transaction);

                var account = _accountService.ViewAccount(accountId);
               
                    account.AccountId = transaction.AccountId;
                    account.Balance = transaction.Balance;

                    _accountService.Update(account);

                return RedirectToPage("/Account/Details", new { accountId = AccountId });

            }
            else
            {  
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            }
          
            return Page();
        }
    }
}
