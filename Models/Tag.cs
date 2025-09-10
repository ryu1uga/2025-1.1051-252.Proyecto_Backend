using System.ComponentModel.DataAnnotations.Schema;

namespace Loop.Models.Common
{
    [Table("Tag")]
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; };
        public DateTime UpdatedAt { get; set; };

        public ICollection<ProductTag>? ProductTags { get; set; }
    }
}