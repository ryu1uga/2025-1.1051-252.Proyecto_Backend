using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("BusinessMember")]
    public class BusinessMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid BusinessId { get; set; }
        public string Role { get; set; } = "";
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };

        
    }
}