namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class GetCommentDto
    {
        public int postId { get; set; }
        public string currentUserId { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string lan { get; set; } = "en";

    }
}
