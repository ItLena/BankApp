using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Transaction
{
    public class TransferModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public TransferModel(ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }

       

        [DisplayName("Amount to transact"), Required, Range(10, 3000)]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        [DisplayName("Comment"), StringLength(50)]
        public string? Comment { get; set; }

        [DisplayName("Bank symbol"), StringLength(10)]
        public string? Bank { get; set; }

        [DisplayName("Account number"), StringLength(20)]
        public string? AccountTo { get; set; }

        public void OnGet(int accountId)
        {
            var account = _accountService.ViewAccount(accountId);
            AccountId = accountId;
            Balance = Balance;
        }
        public IActionResult OnPost(int accountId)
        {
            if (ModelState.IsValid)
            {
                var transaction = new Models.Transaction
                {
                    Operation = "Collection from Another Bank",
                    Type = "Credit",
                    Amount = Amount,
                    Bank = Bank,
                    Date = DateTime.UtcNow,
                    Balance = Balance,
                    Symbol = Comment,
                    Account = AccountTo,
                    AccountId = accountId,
                };
                int transactionId = _transactionService.SaveNew(transaction);

                var account = new Models.Account
                {
                    AccountId = transaction.AccountId,
                    Balance = transaction.Balance
                };
                accountId = _accountService.SaveNew(account);

                return RedirectToPage("/Account/Details");

            }
            return Page();
        }
    }
}
