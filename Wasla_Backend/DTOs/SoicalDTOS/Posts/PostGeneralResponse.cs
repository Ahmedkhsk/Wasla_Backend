namespace Wasla_Backend.DTOs
{
    public class PostGeneralResponse
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string profilePhoto { get; set; }
        public int postId { get; set; }
        public string? content { get; set; }
        public List<string>? files { get; set; }
        public int numberofReactss { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
