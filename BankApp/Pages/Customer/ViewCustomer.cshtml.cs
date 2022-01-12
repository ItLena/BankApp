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

        public ViewCustomerModel(ICustomerService customerService, IAccountService accountService, BankAppDataContext context)
        {
            _customerService = customerService;
            _accountService = accountService;
            _context = context;
        }
                
         
            public int CustomerId { get; set; }
            public string NationalCode { get; set; }
            public string Gender { get; set; } = null!;
            public DateTime? Birthday { get; set; }
            public string? NationalId { get; set; }
            public string? Email { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
       
        public class AccountItem
        {
            public int AccountId { get; set; }
            public decimal Balance { get; set; }

        }

        public List<AccountItem> AccountItems { get; set; }

        public void OnGet(int customerId)
        {
            var customer = _context.Customers.First(c => c.CustomerId == customerId);
            CustomerId = customer.CustomerId;
            Name = customer.Givenname + " " + customer.Surname;
            Gender = customer.Gender;
            Birthday = customer.Birthday;
            NationalId = customer.NationalId;
            Address = customer.Streetaddress + " " + customer.City + " " + customer.Zipcode;
            Phone = "(" + customer.Telephonecountrycode + ")" + customer.Telephonenumber;
            Email = customer.Emailaddress;
            NationalCode = customer.CountryCode;

        }
    }
}
