namespace Wasla_Backend.DTOs.Authentication
{
    public class LoginDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } 
    }
}
