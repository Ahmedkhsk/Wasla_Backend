namespace Wasla_Backend.DTOs.GymDTOS
{
    public class UpdateProfileGym
    {
        [EmailAddress]
        public string gmail { get; set; }
        public string businessName { get; set; }
        public string ownerName { get; set; }
        public string description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<string> phones { get; set; }
        public IFormFile? photo { get; set; }
        public List<IFormFile>? photos { get; set; }
    }
}
