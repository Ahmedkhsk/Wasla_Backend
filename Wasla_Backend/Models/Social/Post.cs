namespace Wasla_Backend.Models.Social
{
    public class Post : Social
    {
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
