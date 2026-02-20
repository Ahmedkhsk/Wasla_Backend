

using Wasla_Backend.Models.GeneralModel;

namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IPaymentService
    {
        Task<(Payment Payment, string RedirectUrl)> ProcessPaymentAsync(CreatePaymentDto createPaymentDto);
        Task<Payment> UpdateOrderSuccess(string transactionId);

        Task<Payment> UpdateOrderFailed(string transactionId);

        string ComputeHmacSHA512(string data, string secret);
    }
}