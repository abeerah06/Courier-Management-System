using CourierApp.Core.Models;

namespace CourierApp.Core.Contracts
{
    public interface ICustomerService
    {
        ValidationResult Validate(Customer customer);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer? GetById(int id);
        List<Customer> GetAll();
        Task<List<Customer>> GetAllAsync();
        List<Customer> Search(string query);
        Task<List<Customer>> SearchAsync(string query);
    }
}
