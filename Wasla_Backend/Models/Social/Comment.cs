namespace Wasla_Backend.Models.Social
{
    public class Comment : Social
    {
        public string? file { get; set; }
        public int postId { get; set; }

        [ForeignKey("postId")]
        public Post post { get; set; }
    }
}
