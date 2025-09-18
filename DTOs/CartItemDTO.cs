namespace Loop.DTOs.Common
{
    public class CartItemDTO
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public float Price { get; set; } = 0;
    }
}