namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class PostRespnse
    {
        public int postId { get; set; }
        public string? content { get; set; }
        public List<string>? files { get; set; }
        public int numberofReacts { get; set; }
        public int numberofSaves { get; set; }
        public bool isLoved { get; set; }
        public bool isSaved { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

    }
}
