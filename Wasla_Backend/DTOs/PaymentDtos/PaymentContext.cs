namespace Wasla_Backend.DTOs
{
    public class PaymentContext
    {
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public string ServiceProviderId { get; set; }
        public string UserId { get; set; }
    }
}
