using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Business")]
    public class Business
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string TaxId { get; set; } = "";
        public int State { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<BusinessMember>? BusinessMembers { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Payout>? Payouts { get; set; }
    }
}