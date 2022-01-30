using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Pages
{
    [Authorize]
    public class LandingModel : PageModel
    {
        
        private readonly ICustomerService _customerService;
        private readonly BankAppDataContext _context;

        public LandingModel(ICustomerService customerService, BankAppDataContext context)
        {
            _customerService = customerService;
            _context = context; 
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

        [BindProperty]
        public int AmmountCustomers { get; set; }

        [BindProperty]
        public int AmmountAccounts { get; set; }

        [BindProperty]
        public decimal SumOfAllAccountsBalance { get; set; }

        [DataType(DataType.Date)]
        public DateTime ActualDate{ get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));


        public void OnGet(string sortColumn, string sortOrder, string searchPhrase, int pageSize, int currentPage)
        {
            SearchPhrase = searchPhrase;
            SortColumn= sortColumn;
            SortOrder= sortOrder;
            ActualDate= DateTime.Now;

            CurrentPage = currentPage;
            PageSize = pageSize;
            Count = _customerService.GetCount();

            AmmountCustomers = _context.Customers.Select(c => c.CustomerId).Distinct().Count();

            AmmountAccounts = _context.Accounts.Select(c => c.AccountId).Distinct().Count();
            SumOfAllAccountsBalance = _context.Accounts.Select(c => c.Balance).Sum();

            if (searchPhrase != null)
            {
                Items = _customerService.GetCustomers(sortColumn, sortOrder, pageSize, currentPage)
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
