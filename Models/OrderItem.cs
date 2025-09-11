using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("OrderItem")]
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid BusinessId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public float Price { get; set; } = 0;
        public int PaymentStatus { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order? Order { get; set; }
        public Business? Business { get; set; }
        public Product? Product { get; set; }
    }
}