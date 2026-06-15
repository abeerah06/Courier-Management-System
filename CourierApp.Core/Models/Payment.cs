namespace CourierApp.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string TrackingNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Unpaid";
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
