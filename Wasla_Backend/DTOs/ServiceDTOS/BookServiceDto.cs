namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class BookServiceDto
    {
        public string UserId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceProviderId { get; set; }
        public double Price { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }
    }
}
