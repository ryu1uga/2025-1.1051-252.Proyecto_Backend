using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Address")]
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public string Line1 { get; set; } = "";
        public string Line2 { get; set; } = "";
        public string City { get; set; } = "";
        public string Region { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}