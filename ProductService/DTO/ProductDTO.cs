namespace Loop.DTOs.Common
{
    public class ProductDTO
    {
        public Guid BusinessId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Status { get; set; } = 0;
        public float Price { get; set; } = 0;
    }
}