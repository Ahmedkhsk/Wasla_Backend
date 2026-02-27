namespace Wasla_Backend.Models.Social
{
    public class Post
    {
        public int id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }

        public string? content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int numberOfReacts { get; set; } = 0;
        public List<string>? files { get; set; }

        public ICollection<Comment> comments { get; set; }
    }
}
