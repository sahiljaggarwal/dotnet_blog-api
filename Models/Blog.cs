using System.ComponentModel.DataAnnotations;

namespace BlogPortal.Models
{
    public class Blog: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int AuthorId { get; set; }

        public User Author { get; set; } = null!;
    }
}