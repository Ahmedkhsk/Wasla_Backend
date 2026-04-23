using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        public Task<List<UserPaymentDto>> GetAllPayment(string ResidentId);
    }
}
