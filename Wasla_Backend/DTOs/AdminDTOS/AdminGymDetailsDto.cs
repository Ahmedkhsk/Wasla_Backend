namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminGymDetailsDto(Gym gym)
    {
        public string businessName { get; set; } = gym.BusinessName;
        public string email { get; set; } = gym.Email;
        public string description { get; set; } = gym.Description;
        public List<string> phones { get; set; } = gym.phones;
        public List<string> images { get; set; } = gym.images;

    }
}
