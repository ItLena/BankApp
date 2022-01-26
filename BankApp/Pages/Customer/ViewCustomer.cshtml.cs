using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankApp.Pages.Customer
{
    public class ViewCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly BankAppDataContext _context;

        public ViewCustomerModel(ICustomerService customerService, BankAppDataContext context, IAccountService accountService)
        {
            _customerService = customerService;
            _context = context;
            _accountService = accountService;
        }
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
            public string NationalCode { get; set; }
            public string Gender { get; set; } = null!;
            public DateTime? Birthday { get; set; }
            public string? NationalId { get; set; }
            public string? Email { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public decimal TotalBalance { get; set; }
        public class AccountItem
        {
            public int AccountId { get; set; }
            public decimal Balance { get; set; }
            public string Type { get; set; }
        }
        
        public List<AccountItem> AccountItems { get; set; }
       
        public void OnGet(int customerId)
        {
            var customer = _customerService.ViewCustomer(customerId);
            CustomerId = customer.CustomerId;
            Name = customer.Givenname + " " + customer.Surname;
            Gender = customer.Gender;
            Birthday = customer.Birthday;
            NationalId = customer.NationalId;
            Address = customer.Streetaddress + " " + customer.City + " " + customer.Zipcode;
            Phone = "(" + customer.Telephonecountrycode + ")" + customer.Telephonenumber;
            Email = customer.Emailaddress;
            NationalCode = customer.CountryCode;

            AccountItems = (from a in _context.Accounts
                            join d in _context.Dispositions on a.AccountId equals d.AccountId
                            join c in _context.Customers on d.CustomerId equals c.CustomerId
                            where c.CustomerId == customerId
                select new AccountItem
                {
                    AccountId = a.AccountId,
                    Balance = a.Balance,
                    Type = d.Type
                }).ToList();

            foreach (var item in AccountItems)
            {
                TotalBalance = item.Balance + TotalBalance;
            }
        }

        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var account = new Models.Account
                {
                    Frequency = "monthly",
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
                
            }

            this.OnGet(customerId);
            return Page();
        }
    }
}
