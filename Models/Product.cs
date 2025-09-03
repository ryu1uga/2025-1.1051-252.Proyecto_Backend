using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Product")]
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BusinessId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Status { get; set; }; = 0;
        public float Price { get; set; }; = 0;
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}