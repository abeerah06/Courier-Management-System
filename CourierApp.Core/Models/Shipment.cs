namespace CourierApp.Core.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int? DriverId { get; set; }
        public string DriverName { get; set; } = "Unassigned";
        public string TrackingNo { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal WeightKg { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime PickupDate { get; set; } = DateTime.Today;
        public DateTime? DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
