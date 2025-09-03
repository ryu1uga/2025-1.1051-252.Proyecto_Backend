using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Order")]
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public int TotalAmount { get; set; }; = 0;
        public int Status { get; set; }; = 0;
        public int PaymentStatus { get; set; }; = 0;
        public int ShippingStatus { get; set; }; = 0;
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}