namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class UpdateCommentDto
    {
        public int commentId { get; set; }
        public IFormFile? file { get; set; }
        public string? content { get; set; }

    }
}
