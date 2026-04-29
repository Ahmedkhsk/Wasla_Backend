using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByPaymobOrderIdAsync(string paymobOrderId);
        Task<Payment?> GetByEntityAsync(EntityType entityType, int entityId, PaymentStatus? status = null);
        Task<List<UserPaymentDto>> GetAllPaymentsByResidentAsync(string residentId);

        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<BaseBooking?> GetBookingByIdAsync(int bookingId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<PaymentStatusResponse> GetPaymentStatusAsync(EntityType entityType, int entityId);


        Task UpdateOrderOnSuccessAsync(int orderId, string transactionId);
        Task UpdateOrderOnFailureAsync(int orderId);
        Task UpdateOrderOnRefundAsync(int orderId);

        Task UpdateBookingOnSuccessAsync(int bookingId, string transactionId);
        Task UpdateBookingOnFailureAsync(int bookingId);
        Task UpdateBookingOnRefundAsync(int bookingId);
    }
}