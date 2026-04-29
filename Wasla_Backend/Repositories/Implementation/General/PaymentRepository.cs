using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Repositories.Implementation.General
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(Context context) : base(context) { }


        public async Task<Payment?> GetByPaymobOrderIdAsync(string paymobOrderId)
            => await _context.Payment
                .FirstOrDefaultAsync(p => p.PaymobOrderId == paymobOrderId);

        public async Task<Payment?> GetByEntityAsync(EntityType entityType, int entityId, PaymentStatus? status = null)
            => await _context.Payment
                .FirstOrDefaultAsync(p =>
                    p.entityType == entityType &&
                    p.entityId == entityId );

        public async Task<List<UserPaymentDto>> GetAllPaymentsByResidentAsync(string residentId)
            => await _context.Payment
                .Where(p => p.ResidentId == residentId)
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new UserPaymentDto
                {
                    ServiceProviderName = p.ServiceProvider.FullName,
                    TotalAmount = (double)p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    ServiceType = p.ServiceType,
                    entityType = p.entityType,
                    EntityId = p.entityId,
                })
                .ToListAsync();


        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<BaseBooking?> GetBookingByIdAsync(int bookingId)
            => await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == bookingId);

        public async Task<Order?> GetOrderByIdAsync(int orderId)
            => await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId);


        public async Task UpdateOrderOnSuccessAsync(int orderId, string transactionId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId)
                ?? throw new NotFoundException(LocalizationKey.OrderNotFound);

            if (order.paymentStatus == PaymentStatus.Completed)
                return;

            order.paymentStatus = PaymentStatus.Completed;
            order.status = OrderStatus.Paid;
            order.transactionId = transactionId;

            var cart = await _context.Carts
                .Include(c => c.items)
                .FirstOrDefaultAsync(c =>
                    c.residentId == order.residentId &&
                    c.restaurantId == order.restaurantId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.items);
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderOnFailureAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId)
                ?? throw new NotFoundException(LocalizationKey.OrderNotFound);

            if (order.paymentStatus == PaymentStatus.Completed)
                return;

            order.paymentStatus = PaymentStatus.Failed;
            order.status = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderOnRefundAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId)
                ?? throw new NotFoundException(LocalizationKey.OrderNotFound);

            order.paymentStatus = PaymentStatus.Refunded;

            await _context.SaveChangesAsync();
        }


        public async Task UpdateBookingOnSuccessAsync(int bookingId, string transactionId)
        {
            var payment = await GetByEntityAsync(EntityType.booking, bookingId);
            if (payment == null) return;

            payment.Status = PaymentStatus.Completed;
            payment.PaymobTransactionId = transactionId;

            var booking = await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking != null)
                booking.IsPaid = true;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingOnFailureAsync(int bookingId)
        {
            var payment = await GetByEntityAsync(EntityType.booking, bookingId);
            if (payment == null) return;

            payment.Status = PaymentStatus.Failed;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingOnRefundAsync(int bookingId)
        {
            var payment = await GetByEntityAsync(EntityType.booking, bookingId);
            if (payment == null) return;

            payment.Status = PaymentStatus.Refunded;

            var booking = await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking != null)
                booking.IsPaid = false;

            await _context.SaveChangesAsync();
        }

        public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(EntityType entityType, int entityId)
        {
            return await _context.Payment
                .Where(p => p.entityType == entityType && p.entityId == entityId)
                .Select(p => new PaymentStatusResponse
                {
                    status = p.Status.ToString(),
                    isPaid = p.Status == PaymentStatus.Completed,
                    paymentMethod = p.PaymentMethod.ToString(),
                    amount = p.Amount,
                    paymobTransactionId = p.PaymobTransactionId
                }).FirstOrDefaultAsync()
                ;
        }
    }
}