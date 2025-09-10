using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Customer")]
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Cart? Cart { get; set; }
        public User? User { get; set; }

        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}