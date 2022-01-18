using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly BankAppDataContext _context;

        public DetailsModel(IAccountService accountService, BankAppDataContext context)
        {
            _accountService = accountService;
            _context = context;
        }


        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public decimal Loan { get; set; }
        public decimal TotalBalance { get; set; }


        public class TransactionItem
        {
            public int TransactionId { get; set; }
            public int AccountId { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; } = null!;
            public string Operation { get; set; } = null!;
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string? Symbol { get; set; }
            public string? Bank { get; set; }
            public string? Account { get; set; }
        }

        public List<TransactionItem> TransactionItems { get; set; }

        public void OnGet(int accountId)
        {
            var account = _accountService.ViewAccount(accountId);
            AccountId = account.AccountId;
            TotalBalance = account.Balance + TotalBalance;
            
           

            //AccountItems = (from a in _context.Accounts
            //                join d in _context.Dispositions on a.AccountId equals d.AccountId
            //                join c in _context.Customers on d.CustomerId equals c.CustomerId
            //                where c.CustomerId == customerId
            //                select new AccountItem
            //                {
            //                    AccountId = a.AccountId,
            //                    Balance = a.Balance,
            //                    Type = d.Type
            //                }).ToList();

        }
    }

}
