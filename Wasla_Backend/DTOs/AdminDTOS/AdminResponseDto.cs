namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminResponseDto
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
    }
}
