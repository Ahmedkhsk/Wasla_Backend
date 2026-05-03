namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AddAdminDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string? FullName { get; set; }
        public char Gender { get; set; }
        public string? Phone { get; set; }
    }
}
