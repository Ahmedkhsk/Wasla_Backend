namespace Wasla_Backend.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string ServiceProviderId { get; set; }

  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}