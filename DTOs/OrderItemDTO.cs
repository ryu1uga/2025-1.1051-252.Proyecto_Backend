namespace Loop.DTOs.Common
{
    public class OrderItemDTO
    {
        public int Quantity { get; set; } = 0;
        public float Price { get; set; } = 0;
        public int PaymentStatus { get; set; } = 0;
    }
}