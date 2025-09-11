using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Order")]
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public float TotalAmount { get; set; } = 0;
        public int Status { get; set; } = 0;
        public int PaymentStatus { get; set; } = 0;
        public int ShippingStatus { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Customer? Customer { get; set; }
        public Address? Address { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}