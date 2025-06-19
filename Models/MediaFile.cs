namespace BlogPortal.Models
{
    public class MediaFile : BaseEntity
    {
        public int? Id { get; set; }
        public string? PublicId { get; set; }
        public string? Url { get; set; }
        public string? Format { get; set; }
        public string? Folder { get; set; } = "DEFAULT";
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<string> BlogIds { get; set; } = new();
    }
}