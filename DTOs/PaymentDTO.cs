namespace Loop.DTOs.Common
{
    public class PaymentDTO
    {
        public Guid OrderId { get; set; }
        public string Provider { get; set; } = "";
        public string ProviderRef { get; set; } = "";
        public float Amount { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime PaidAt { get; set; }
    }
}