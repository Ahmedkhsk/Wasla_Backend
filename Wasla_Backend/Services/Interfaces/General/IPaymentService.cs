using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IPaymentService
    {
        Task<(Payment payment, string redirectUrl)> ProcessPaymentAsync(CreatePaymentDto dto);

        Task HandlePaymentCallback(string merchantOrderId, bool isSuccess, bool isRefunded, string transactionId);

        Task<bool> RefundPaymentAsync(RefundDto dto);

        Task<Payment> GetPaymentStatusAsync(EntityType entityType, int entityId);

        string ComputeHmacSHA512(string data, string secret);
        public Task<List<UserPaymentDto>> GetAllPayment(string ResidentId);

    }
}