namespace BlogPortal.Models
{
    public class UserPayload
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public long Nbf { get; set; }  
        public long Exp { get; set; } 
        public long Iat { get; set; } 
    }
}