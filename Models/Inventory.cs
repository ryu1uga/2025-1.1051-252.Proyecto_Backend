using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Inventory")]
    public class Inventory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }; = 0;
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}