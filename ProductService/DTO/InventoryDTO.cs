namespace Loop.DTOs.Common
{
    public class InventoryDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
    }
}