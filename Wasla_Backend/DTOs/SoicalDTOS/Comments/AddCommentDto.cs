namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class AddCommentDto
    {
        public string userId { get; set; }
        public string? content { get; set; }
        public int postId { get; set; }
        public IFormFile? file { get; set; }

    }
}
