namespace CourierApp.Core.Contracts
{
    public interface IDashboardService
    {
        int GetTotalShipments();
        int GetShipmentsThisMonth();
        int GetShipmentsThisWeek();
        int GetShipmentsToday();
        Dictionary<string, int> GetMonthlyShipments(int months);
        Dictionary<string, int> GetStatusBreakdown();
        Task<int> GetTotalShipmentsAsync();
        Task<Dictionary<string, int>> GetMonthlyShipmentsAsync(int months);
    }
}
