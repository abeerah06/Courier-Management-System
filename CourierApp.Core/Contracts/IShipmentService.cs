using CourierApp.Core.Models;

namespace CourierApp.Core.Contracts
{
    public interface IShipmentService
    {
        ValidationResult Validate(Shipment s);
        void Add(Shipment s);
        void Update(Shipment s);
        void Delete(int id);
        void UpdateStatus(int id, string status);
        Shipment? GetById(int id);
        List<Shipment> GetAll();
        Task<List<Shipment>> GetAllAsync();
        List<Shipment> Search(string query, string statusFilter = "All");
        Task<List<Shipment>> SearchAsync(string query, string statusFilter = "All");
        string GenerateTrackingNo();
    }
}
