using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("CartItem")]
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }; = 0;
        public int Price { get; set; }; = 0;
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}