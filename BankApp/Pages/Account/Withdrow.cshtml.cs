using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages.Account
{
    [BindProperties]
    public class WithdrowModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public WithdrowModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }
        public void OnGet(int accountId)
        {
            var account = _accountService.ViewAccount(accountId);
            AccountId = account.AccountId;
            Balance = account.Balance;
        }

        public IActionResult OnPost (int accountId)
        {
            var account = _accountService.ViewAccount(accountId);
            if (ModelState.IsValid)
            {
               if (account.Balance > Amount)
                {
                    account.Balance = Balance - Amount;
                }
                else
                {
                    ModelState.AddModelError("Amount", "Amount is more than ballance");
                }
                return Page();
            }

            return Page();
        }
    }
}
