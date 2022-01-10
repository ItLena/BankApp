using BankApp.Models;

namespace BankApp.Services
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers(string sortColumn, string sortOrder);
    }
}
