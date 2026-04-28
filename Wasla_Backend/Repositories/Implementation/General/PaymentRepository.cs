
using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Repositories.Implementation.General
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(Context context) : base(context)
        {
        }

        public async Task<List<UserPaymentDto>> GetAllPayment(string ResidentId)
        {
            return await _context.Payment.Where(p => p.ResidentId == ResidentId)
                .OrderByDescending(p=>p.PaymentDate)
                .Select(p => new UserPaymentDto
            {
                ServiceProviderName = p.ServiceProvider.FullName,
                TotalAmount=(double)p.Amount,
                PaymentDate=p.PaymentDate,
                PaymentMethod=p.PaymentMethod,
                Status=p.Status,
                ServiceType= p.ServiceType,
                entityType=p.entityType,


            }).ToListAsync();
        }
    }
}
