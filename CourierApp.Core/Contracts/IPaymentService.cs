using CourierApp.Core.Models;

namespace CourierApp.Core.Contracts
{
    public interface IPaymentService
    {
        void Add(Payment p);
        void Update(Payment p);
        void Delete(int id);
        void UpdateStatus(int id, string status);
        Payment? GetById(int id);
        Payment? GetByShipmentId(int shipmentId);
        List<Payment> GetAll();
        Task<List<Payment>> GetAllAsync();
        List<Payment> Search(string query);
        Task<List<Payment>> SearchAsync(string query);
    }
}
