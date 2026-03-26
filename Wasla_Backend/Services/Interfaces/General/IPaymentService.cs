

using Wasla_Backend.Models.GeneralModel;

namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IPaymentService
    {
        Task<(Payment Payment, string RedirectUrl)> ProcessPaymentAsync(CreatePaymentDto createPaymentDto);

        Task<Payment> UpdateOrderSuccess(string transactionId, string paymobTransactionId = null);

        Task<Payment> UpdateOrderFailed(string transactionId);

        Task<Payment> UpdateOrderRefunded(string transactionId);

        Task<bool> RefundPaymentAsync(int bookingId, string lan = "en");

        string ComputeHmacSHA512(string data, string secret);
        Task<Payment> GetPaymentByBookingIdAsync(int bookingId);

    }
}