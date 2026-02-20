namespace Wasla_Backend.DTOs
{
    public class PaymentInitResponse
    {
        public bool IsSuccess { get; set; }
        public string? RedirectUrl { get; set; }
        public string? ReferenceCode { get; set; }
        public string? ProviderTransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
