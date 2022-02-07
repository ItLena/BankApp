using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BankApp.Pages.Customer
{
    public enum Genders
    {
        Male,
        Female,
        Underfind
    }
    public enum Freqwence
    {
        Monthly,
        Weekly
    }

    [Authorize]
    [BindProperties]
    public class NewModel : PageModel
    {
        
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly BankAppDataContext _context;

        public NewModel(ICustomerService customerService, IAccountService accountService, BankAppDataContext context)
        {
            _customerService = customerService;
            _accountService = accountService;
            _context = context;
        }

        public int CustomerId { get; set; }

        [DisplayName("Gender"), Required, StringLength(10)]
        public string Gender { get; set; } = null!;

        [DisplayName("First Name"), Required, StringLength(50)]
        public string Givenname { get; set; } = null!;

        [DisplayName("Last Name"), Required, StringLength(50)]
        public string Surname { get; set; } = null!;

        [DisplayName("Street"), Required, StringLength(50)]
        public string Streetaddress { get; set; } = null!;

        [DisplayName("City"), Required, StringLength(50)]
        public string City { get; set; } = null!;

        [DisplayName("Zip Code"), Required, StringLength(20)]
        public string Zipcode { get; set; } = null!;

        [DisplayName("Country"), Required, StringLength(50)]
        public string Country { get; set; } = null!;

        [DisplayName("Country Code"), Required, StringLength(5)]
        public string CountryCode { get; set; } = null!;

        [DisplayName("Birthday"), Required, DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [DisplayName("Induviduel Nationel Number"), Required, StringLength(20)]
        public string? NationalId { get; set; }

        [DisplayName("Phone Country Code"), Required, StringLength(10)]
        public string? Telephonecountrycode { get; set; }

        [DisplayName("Phone Number"), Required, DataType(DataType.PhoneNumber)]
        public string? Telephonenumber { get; set; }

        [DisplayName("Email"), Required, DataType(DataType.EmailAddress)]
        public string? Emailaddress { get; set; }
        public int AccountId { get; set; }

        [DisplayName("Account Frequency"), Required, StringLength(20)]
        public string Frequency { get; set; }

        public DateTime Created { get; set; }

        public void OnGet()
        {
           
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new Models.Customer
                {
                    Givenname = Givenname,
                    Surname = Surname,
                    Gender = Gender,
                    Emailaddress = Emailaddress,
                    Telephonecountrycode = Telephonecountrycode,
                    Telephonenumber = Telephonenumber,
                    Streetaddress = Streetaddress,
                    Zipcode = Zipcode,
                    City = City,
                    Country = Country,
                    CountryCode = CountryCode,
                    Birthday = Birthday,
                    NationalId = NationalId,
                };
                
                int customerId = _customerService.SaveNew(customer);
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
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
