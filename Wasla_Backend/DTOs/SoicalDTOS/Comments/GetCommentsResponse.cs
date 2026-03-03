namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class GetCommentsResponse
    {
        public int commentId { get; set; }
        public string? content { get; set; }
        public bool isLove { get; set; }
        public int numberOfLikes { get; set; }
        public string? file { get; set; }
        public string userName { get; set; }
        public string userProfile { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
