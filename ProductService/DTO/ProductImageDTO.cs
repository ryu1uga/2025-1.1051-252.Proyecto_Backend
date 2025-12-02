namespace Loop.DTOs.Common
{
    public class ProductImageDTO
    {
        public Guid ProductId { get; set; }
        public string Url { get; set; } = "";
        public int ImageOrder { get; set; } = 0;
    }
}