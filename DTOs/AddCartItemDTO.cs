namespace Loop.DTOs.Common
{
    public class AddCartItemDTO
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
