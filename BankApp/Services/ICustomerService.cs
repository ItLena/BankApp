using BankApp.Models;

namespace BankApp.Services
{
    public interface ICustomerService
    {
        public IEnumerable<Customer> GetCustomers(string sortColumn, string sortOrder, int pageSize, int currentPage);
        Customer ViewCustomer(int customerId);
        int SaveNew(Customer customer);

        void Update(Customer customer);
        public int GetCount();
    }
}
