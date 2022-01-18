using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages.Customer
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        
        public EditModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public int CustomerId { get; set; }

        [DisplayName("Gender"), Required, StringLength(10)]
        public string Gender { get; set; } = null!;

        [DisplayName ("First Name"), Required, StringLength(50)]
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


        public void OnGet(int customerId)
        {
            var customer = _customerService.ViewCustomer(customerId);

                CustomerId = customer.CustomerId;
                Givenname = customer.Givenname;
                Surname = customer.Surname;
                Gender = customer.Gender;
                Emailaddress = customer.Emailaddress;
                Telephonecountrycode = customer.Telephonecountrycode;
                Telephonenumber = customer.Telephonenumber;
                Streetaddress = customer.Streetaddress;
                Zipcode = customer.Zipcode;
                City = customer.City;
                Country = customer.Country;
                CountryCode = customer.CountryCode;
                Birthday = customer.Birthday;
                NationalId = customer.NationalId;
        }  

        public IActionResult OnPost(int customerId)
        {
            
            if (ModelState.IsValid)
            {
                var customer = _customerService.ViewCustomer(customerId);

                   customer.CustomerId = CustomerId;
                   customer.Givenname = Givenname;
                   customer.Surname = Surname;
                   customer.Gender = Gender;
                   customer.Emailaddress = Emailaddress;
                   customer.Telephonecountrycode = Telephonecountrycode;
                   customer.Telephonenumber = Telephonenumber;
                   customer.Streetaddress = Streetaddress;
                   customer.Zipcode = Zipcode;
                   customer.City = City;
                   customer.Country = Country;
                   customer.CountryCode = CountryCode;
                   customer.Birthday = Birthday;
                   customer.NationalId = NationalId;

                    _customerService.Update(customer);
                    return RedirectToPage("Index");
            }
             return Page();
        }
    }
}
