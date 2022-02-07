using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.ControllerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BankAppDataContext _context;
        public ValuesController(BankAppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _context.Customers.Select(c => new Customer
            {
                CustomerId = c.CustomerId,
                NationalId = c.NationalId,
                Givenname = c.Givenname,
                Surname = c.Surname,
                Emailaddress = c.Emailaddress,
                Telephonecountrycode = c.Telephonecountrycode,
                Telephonenumber = c.Telephonenumber,
                City = c.City,
                CountryCode = c.CountryCode,
                Country = c.Country,
            });
        }
    }
}
