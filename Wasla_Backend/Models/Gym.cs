namespace Wasla_Backend.Models
{
    public class Gym : ServiceProvider
    {
        public List<string>? phones { get; set; }
        public string? imagesJson { get; set; }
        
        [NotMapped]
        public List<string> images
        {
            get => imagesJson == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(imagesJson);
            set => imagesJson = JsonSerializer.Serialize(value);
        }
    }
}
