namespace Loop.DTOs.Common
{
    public class ProductImageDTO
    {
        public Guid ProductId { get; set; }
        public Guid BusinessId { get; set; }
        public string Url { get; set; } = "";
        public int Order { get; set; } = 0;
    }
}