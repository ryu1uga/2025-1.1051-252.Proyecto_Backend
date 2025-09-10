using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Payout")]
    public class Payout
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BusinessId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Amount { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Business? Business { get; set; }
    }
}