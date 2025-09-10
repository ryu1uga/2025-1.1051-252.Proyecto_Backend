using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Payment")]
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public string Provider { get; set; } = "";
        public string ProviderRef { get; set; } = "";
        public int Amount { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order? Order { get; set; }
    }
}