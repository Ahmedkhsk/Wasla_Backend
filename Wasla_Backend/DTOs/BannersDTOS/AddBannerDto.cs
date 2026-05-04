namespace Wasla_Backend.DTOs
{
    public class AddBannerDto
    {
        public IFormFile image { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }
}
