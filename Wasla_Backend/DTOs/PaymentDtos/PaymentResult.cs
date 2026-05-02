namespace Wasla_Backend.DTOs
{
    public class PaymentResult
    {
        public string? paymentUrl { get; set; }
        public PaymentStatus status { get; set; }
    }
}
