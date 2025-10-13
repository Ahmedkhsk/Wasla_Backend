namespace Wasla_Backend.DTOs.Authentication
{
    public class VerificationEmailDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
