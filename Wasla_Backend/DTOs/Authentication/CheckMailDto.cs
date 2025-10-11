namespace Wasla_Backend.DTOs.Authentication
{
    public class CheckMailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
