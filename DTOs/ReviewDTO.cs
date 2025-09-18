namespace Loop.DTOs.Common
{
    public class ReviewDTO
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Rating { get; set; } = 0;
        public string Comment { get; set; } = "";
    }
}