namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class CreatePaymentDto
    {
        public string UserId { get; set; }  
        public string? ServiceProviderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public int entityId { get; set; }
        public EntityType entityType { get; set; }
        public ServiceProviderType ServiceType { get; set; }
    }
}
