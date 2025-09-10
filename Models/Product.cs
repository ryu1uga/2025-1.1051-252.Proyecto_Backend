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

        public Business? Business { get; set; }
        public Category? Category { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Inventory>? Inventories { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<ProductTag>? ProductTags { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}