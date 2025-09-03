using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("ModelName")]
    public class ModelName
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}