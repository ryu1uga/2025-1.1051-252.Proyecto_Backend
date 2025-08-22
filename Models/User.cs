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
        public DateTime UpdatedAt { get; set; }
        public int? VerificationCode { get; set; }
        public DateTime? CodeRegisteredDate { get; set; }
        public DateTime? CodeExpirationDate { get; set; }
        public ICollection<Profile>? Profiles { get; set; }
        public int? Token { get; set; }
    }
} 