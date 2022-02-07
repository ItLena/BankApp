using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Account
{
     public enum Freqwence
    {
        Monthly,
        Weekly
    }
    [Authorize]
    [BindProperties]
    public class NewAccountModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly BankAppDataContext _context;

        public NewAccountModel(IAccountService accountService, BankAppDataContext context)
            {
                _accountService = accountService;
                _context = context;
            }

            public int AccountId { get; set; }
            public int CustomerId { get; set; }

        [DisplayName("Frequency"), Required, StringLength(50)]
            public string Frequency { get; set; } = null!;
            public DateTime Created { get; set; }

            [DisplayName("Ballance"), Required]
            public decimal Balance { get; set; }

            public void OnGet(int customerId)
            {
                CustomerId = customerId;
            }
            public IActionResult OnPost(int customerId)
            {
                if (ModelState.IsValid)
                {
                var account = new Models.Account
                {
                    Frequency = Frequency,
                    Created = DateTime.UtcNow,
                    Balance = 0
                };
                int accountId = _accountService.SaveNew(account);

                var disposition = new Models.Disposition
                {
                    CustomerId = customerId,
                    AccountId = accountId,
                    Type = "OWNER"
                };
                _context.Dispositions.Add(disposition);
                _context.SaveChanges();

                return RedirectToPage("/Customer/ViewCustomer");
                }
                return Page();
            }
        }
    
}

