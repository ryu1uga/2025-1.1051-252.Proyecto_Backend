using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("ProductTag")]
    public class ProductTag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid TagId { get; set; }
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };

        public Product? Product { get; set; }
        public Tag? Tag { get; set; }
    }
}