using CourierApp.Core.Models;

namespace CourierApp.Core.Contracts
{
    public interface IDriverService
    {
        ValidationResult Validate(Driver d);
        void Add(Driver d);
        void Update(Driver d);
        void Delete(int id);
        Driver? GetById(int id);
        List<Driver> GetAll();
        Task<List<Driver>> GetAllAsync();
        List<Driver> GetActive();
        List<Driver> Search(string query);
        Task<List<Driver>> SearchAsync(string query);
    }
}
