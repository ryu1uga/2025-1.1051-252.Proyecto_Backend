namespace Loop.DTOs.Common
{
    public class PayoutDTO
    {
        public Guid BusinessId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public float Amount { get; set; } = 0;
        public int Status { get; set; } = 0;
    }
}