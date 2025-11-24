namespace Wasla_Backend.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string ServiceProviderId { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ServiceProvider ServiceProvider { get; set; }
        public ServiceProviderType ServiceType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
