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
        public int UsertypeId { get; set; };
        public DateTime CreatedAt { get; set; };
    }
} 