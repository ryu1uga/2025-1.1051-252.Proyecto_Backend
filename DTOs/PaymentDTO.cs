namespace Loop.DTOs.Common
{
    public class PaymentDTO
    {
        public string Provider { get; set; } = "";
        public string ProviderRef { get; set; } = "";
        public int Amount { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime PaidAt { get; set; }
    }
}