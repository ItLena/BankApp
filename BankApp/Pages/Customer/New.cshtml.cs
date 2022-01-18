using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class NewModel : PageModel
    {
        
        private readonly ICustomerService _customerService;

        public NewModel(ICustomerService customerService)
        {
            _customerService = customerService;
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
                _customerService.SaveNew(customer);
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
