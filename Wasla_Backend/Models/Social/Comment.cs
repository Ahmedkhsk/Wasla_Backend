namespace Wasla_Backend.Models.Social
{
    public class Comment
    {
        public int id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }

        public int postId { get; set; }

        [ForeignKey("postId")]
        public Post post { get; set; }
        
        public string content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string file { get; set; }
    }
}
