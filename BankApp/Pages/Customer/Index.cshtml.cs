using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace BankApp.Pages.Customer
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public class Item
        {
            public int Id { get; set; }
            public string PersonalNumber { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string City { get; set; }
            public string CountryCode { get; set; }
            
        }
        public List<Item> Items { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public string SearchPhrase { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public void OnGet(string sortColumn, string sortOrder, string searchPhrase, int pageSize, int currentPage)
        {
            SearchPhrase = searchPhrase;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            CurrentPage = currentPage;
            pageSize = 50;
            PageSize = pageSize;
            

            if (searchPhrase != null)
            {

           Items =  _customerService.GetCustomers(sortColumn, sortOrder, pageSize, currentPage)
                .Select(c => new Item
                {
                    Id = c.CustomerId,
                    PersonalNumber = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Email = c.Emailaddress,
                    Phone = "(" + c.Telephonecountrycode + ")" + c.Telephonenumber,
                    City = c.City,
                    CountryCode = c.CountryCode,
                }).Where(x => x.Name.ToLower().Contains(searchPhrase.ToLower()) || x.PersonalNumber.Contains(searchPhrase) || x.CountryCode.ToLower().Contains(searchPhrase.ToLower()) || x.City.ToLower().Contains(searchPhrase.ToLower()))
                .Skip((currentPage) * pageSize).Take(pageSize)
                .ToList();
             }

            else
            {
                Items = _customerService.GetCustomers(sortColumn, sortOrder, pageSize, currentPage)
                .Select(c => new Item
                {
                    Id = c.CustomerId,
                    PersonalNumber = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Email = c.Emailaddress,
                    Phone = "(" + c.Telephonecountrycode + ")" + c.Telephonenumber,
                    City = c.City,
                    CountryCode = c.CountryCode,
                })
                .Skip((currentPage) * pageSize).Take(pageSize)
                .ToList();
            }

             Count = _customerService.GetCount()/pageSize;
        }
    }
}
