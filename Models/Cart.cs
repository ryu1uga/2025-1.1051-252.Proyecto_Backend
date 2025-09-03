using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Cart")]
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}