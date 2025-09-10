namespace Loop.DTOs.Common
{
    public class OrderDTO
    {
        public Guid AddressId { get; set; }
        public int TotalAmount { get; set; }; = 0;
        public int Status { get; set; }; = 0;
        public int PaymentStatus { get; set; }; = 0;
        public int ShippingStatus { get; set; }; = 0;
    }
}