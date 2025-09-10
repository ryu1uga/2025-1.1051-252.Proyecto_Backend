namespace Loop.DTOs.Common
{
    public class PayoutDTO
    {
        public DateTime From { get; set; };
        public DateTime To { get; set; };
        public int Amount { get; set; }; = 0;
        public int Status { get; set; }; = 0;
    }
}