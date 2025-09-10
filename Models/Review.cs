using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Review")]
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Rating { get; set; }; = 0;
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };

        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
    }
}