using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Category")]
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };
    }
}