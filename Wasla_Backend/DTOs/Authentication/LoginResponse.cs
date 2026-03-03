namespace Wasla_Backend.DTOs.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; } 
        public string UserId { get; set; }
        public string Role { get; set; }
        public string profilePhoto { get; set; }
        public bool IsVerfied { get; set; }
        public bool IsCompletedRegister { get; set; }
        public UserStatus statue { get; set; }

    }
}
