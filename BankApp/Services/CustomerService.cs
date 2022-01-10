using BankApp.Models;

namespace BankApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankAppDataContext _context;

        public CustomerService(BankAppDataContext context)
        {
            _context = context;
        }

        public List<Customer> GetCustomers(string sortColumn, string sortOrder)
        {
            var query = _context.Customers.AsQueryable();

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(r => r.Country);
                else
                    query = query.OrderByDescending(r => r.Country);

            else if (sortColumn == "Name" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "desc")
                    query = query.OrderByDescending(r => r.Givenname ).ThenBy(r=>r.Surname);
                else
                    query = query.OrderBy(r => r.Givenname).ThenBy(r=>r.Surname);


            else if (sortColumn == "City")
                if (sortOrder == "asc")
                    query = query.OrderBy(r => r.City);
                else
                    query = query.OrderByDescending(r => r.City);

            return query.ToList();

        }

        
    }
}
