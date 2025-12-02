using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("ProductImage")]
    public class ProductImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public string Url { get; set; } = "";
        public int ImageOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Product? Product { get; set; }
    }
}