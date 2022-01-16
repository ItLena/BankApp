using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages
{
    public class LandingModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public LandingModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public class Item
        {
            public int Id { get; set; }
            public string PersonalNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Street { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
        }
        public List<Item> Items { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public string SearchPhrase { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime ActualDate{ get; set; }
        public void OnGet(string sortColumn, string sortOrder, string searchPhrase)
        {
            SearchPhrase = searchPhrase;
            SortColumn= sortColumn;
            SortOrder= sortOrder;
            ActualDate= DateTime.Now;

            if (searchPhrase != null)
            {
                Items = _customerService.GetCustomers(sortColumn, sortOrder)
                .Select(c => new Item
                {
                    Id = c.CustomerId,
                    PersonalNumber = c.NationalId,
                    FirstName = c.Givenname,
                    LastName = c.Surname,
                    Street = c.Streetaddress,
                    ZipCode = c.Zipcode,
                    City = c.City,
                    Country = c.Country,
                }).Where(x => x.FirstName.ToLower().Contains(searchPhrase.ToLower()) || x.LastName.ToLower().Contains(searchPhrase.ToLower()) || x.PersonalNumber.Contains(searchPhrase) || x.Country.ToLower().Contains(searchPhrase.ToLower())).ToList();
            }
           
            
        }


    }
}
