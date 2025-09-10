using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("User")]
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public int UserType { get; set; } = 0; //0 si es cliente, 1 si es admin
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<BusinessMember>? BusinessMembers { get; set; }
    }
}