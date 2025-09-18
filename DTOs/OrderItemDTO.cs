namespace Loop.DTOs.Common
{
    public class OrderItemDTO
    {
        public Guid OrderId { get; set; }
        public Guid BusinessId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public float Price { get; set; } = 0;
        public int PaymentStatus { get; set; } = 0;
    }
}