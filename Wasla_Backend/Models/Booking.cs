namespace Wasla_Backend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public String UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        public string ServiceProviderId { get; set; }
        public double Price { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
