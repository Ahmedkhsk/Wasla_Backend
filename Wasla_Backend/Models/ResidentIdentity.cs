namespace Wasla_Backend.Models
{
    public class ResidentIdentity
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
