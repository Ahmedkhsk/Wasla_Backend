namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class CreatePaymentDto
    {
        public string UserId { get; set; }  
        public string ServiceProviderId { get; set; }
        public int ServiceId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }
        public int BookingId { get; set; }
    }
}
