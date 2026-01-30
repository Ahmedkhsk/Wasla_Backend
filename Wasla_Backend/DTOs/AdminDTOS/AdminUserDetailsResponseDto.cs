namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminUserDetailsResponseDto
    {
        public string role { get; set; }
        public AdminUserBaseDetailsDto userBase { get; set; }
        public object details { get; set; }
    }
}
