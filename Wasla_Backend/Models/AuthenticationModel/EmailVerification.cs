namespace Wasla_Backend.Models
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string VerificationCode { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } 
    }
}