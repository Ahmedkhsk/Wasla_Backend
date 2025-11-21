namespace Wasla_Backend.DTOs.Authentication
{
    public class EditProfileDto
    {
        public string id { get; set; }
        public string fullname { get; set; }
        public IFormFile? image { get; set; }
        public string phone { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
