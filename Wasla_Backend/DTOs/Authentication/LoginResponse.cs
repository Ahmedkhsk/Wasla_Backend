namespace Wasla_Backend.DTOs.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; } 
        public string UserId { get; set; }
        public string Role { get; set; }
        public Boolean IsVerfied { get; set; }
        public Boolean IsCompletedRegister { get; set; }
        public UserStatus statue { get; set; }

    }
}
