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

        public string? fileJson { get; set; }

        [NotMapped]
        public List<string> files
        {
            get => fileJson == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(fileJson);
            set => fileJson = JsonSerializer.Serialize(value);
        }
        public ICollection<Comment> comments { get; set; }
    }
}
