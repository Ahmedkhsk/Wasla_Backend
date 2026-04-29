namespace Wasla_Backend.DTOs.PaymentDtos
{
    public class UserPaymentDto
    {
        public string ServiceProviderName { get; set; }
        public int EntityId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public ServiceProviderType ServiceType { get; set; }
        public EntityType entityType { get; set; }


    }
}
