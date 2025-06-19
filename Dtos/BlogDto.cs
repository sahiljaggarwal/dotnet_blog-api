using System.ComponentModel.DataAnnotations;

namespace BlogPortal.Dtos
{
    public class CreateBlogDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? MediaFileId { get; set; }
    }

    public class QueryBlogDto
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;

        public string? Search { get; set; }
    }
}
