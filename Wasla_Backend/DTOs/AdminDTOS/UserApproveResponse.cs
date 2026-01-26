namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class UserApproveResponse
    {
        public string id { get; set; }
        public string name { get; set; }

        [EmailAddress]
        public string email { get; set; }
        public UserStatus status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
