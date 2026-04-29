using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IPaymentService
    {
        Task<(Payment payment, string redirectUrl)> ProcessPaymentAsync(CreatePaymentDto dto);
        Task HandlePaymentCallbackByPaymobOrderId(string paymobOrderId, bool isSuccess, bool isRefunded, string transactionId);
        Task<bool> RefundPaymentAsync(EntityTypeDto dto);
        Task<PaymentStatusResponse> GetPaymentStatusAsync(EntityType entityType, int entityId);
        Task<List<UserPaymentDto>> GetAllPaymentsAsync(string residentId);
    }
}