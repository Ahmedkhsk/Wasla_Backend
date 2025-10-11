namespace Wasla_Backend.DTOs.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; } 
        public string UserId { get; set; }
        public string Role { get; set; }
        
    }
}
