using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Transaction
{
    [Authorize]
    [BindProperties]
    public class TransferModel : PageModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly BankAppDataContext _context;

        public TransferModel(ITransactionService transactionService, IAccountService accountService, BankAppDataContext context)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _context = context;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
        public string Operation { get; set; } 

        [DisplayName("Amount"), Required, Range(10,10000)]
        public decimal Amount { get; set; }
        
        [DisplayName("Comment"), StringLength(50)]
        public string? Comment { get; set; }

        [DisplayName("Bank name"), StringLength(10)]
        public string? Bank { get; set; }

        [DisplayName("Account number"), StringLength(20)]
        public string? AccountTo { get; set; }

        [DisplayName("Type of transaction"), StringLength(20)]
        public string Type { get; set; }
        public int OrderId { get; set; }

        public void OnGet(int accountId)
        {
           var account = _accountService.ViewAccount(accountId);
            AccountId = accountId;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountId, string operation, string type)
        {
            var accountSaldo = _accountService.ViewAccount(accountId);
            Balance = accountSaldo.Balance;
            Operation = operation;
            Type = type;
           
            if (ModelState.IsValid)
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
               
                var transaction = new Models.Transaction
                {
                    Operation = Operation,
                    Type = Type,
                    Amount = Amount,
                    Bank = Bank,
                    Date = DateTime.UtcNow,
                    Balance = Balance,
                    Symbol = Comment,
                    Account = AccountTo,
                    AccountId = accountId,
                };
                int transactionId = _transactionService.SaveNew(transaction);

                var account = _accountService.ViewAccount(accountId);
                account.AccountId = transaction.AccountId;
                account.Balance = transaction.Balance;

                _accountService.Update(account);

                var permented = new Models.PermenentOrder
                {
                    OrderId = OrderId,
                    Amount = Amount,
                    BankTo = Bank,
                    Symbol = Comment,
                    AccountTo = AccountTo,
                    AccountId = accountId,
                };
                _context.PermenentOrders.Add(permented);
                _context.SaveChanges();

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
