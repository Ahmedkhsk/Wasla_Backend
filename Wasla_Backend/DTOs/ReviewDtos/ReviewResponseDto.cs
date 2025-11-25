namespace Wasla_Backend.DTOs.ReviewDtos
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; }
        public string UserImageUrl { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
  
    }
}
