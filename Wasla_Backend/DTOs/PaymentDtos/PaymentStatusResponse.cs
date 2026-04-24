namespace Wasla_Backend.DTOs.PaymentDtos
{
    public class PaymentStatusResponse
    {
        public string status { get; set; }
        public bool isPaid { get; set; }
        public string paymentMethod { get; set; }
        public decimal amount { get; set; }
        public string? paymobTransactionId { get; set; }
    }
}
