namespace Wasla_Backend.Services.Implementation.General
{
    public class PaymobService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly Context _context;
        private readonly DateTimeHelper _dateTimeHelper;

        public PaymobService(IConfiguration configuration, Context context, DateTimeHelper dateTimeHelper)
        {
            _configuration = configuration;
            _context = context;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<(Payment payment, string redirectUrl)> ProcessPaymentAsync(CreatePaymentDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId))
                throw new BadRequestException(LocalizationKey.ResidentIdRequired);

            var resident = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId)
                ?? throw new NotFoundException(LocalizationKey.ResidentNotFound);

            if (dto.Amount <= 0)
                throw new BadRequestException(LocalizationKey.AmountMustBeGreaterThanZero);

            if (dto.entityType == EntityType.booking)
            {
                var booking = await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == dto.entityId)
                    ?? throw new NotFoundException(LocalizationKey.BookingNotFound);
            }
            else if (dto.entityType == EntityType.order)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == dto.entityId)
                    ?? throw new NotFoundException(LocalizationKey.OrderNotFound);
            }

            var merchantOrderId = $"{dto.entityType}_{dto.entityId}";
            var amountCents = (int)(dto.Amount * 100);

            var names = (resident.FullName ?? "Guest User").Split(' ');
            var firstName = names.Length > 0 ? names[0] : "Guest";
            var lastName = names.Length > 1 ? names[^1] : "User";

            var billingData = new
            {
                first_name = firstName,
                last_name = lastName,
                email = resident.Email,
                phone_number = resident.Phone,
                apartment = "N/A",
                floor = "N/A",
                street = "N/A",
                building = "N/A",
                city = "N/A",
                state = "N/A",
                country = "N/A"
            };

            var payload = new
            {
                amount = amountCents,
                currency = "EGP",
                payment_methods = new[] { int.Parse(DetermineIntegrationId(dto.PaymentMethod)) },
                billing_data = billingData,
                customer = new { id = dto.UserId },
                expiration = 3600,
                merchant_order_id = merchantOrderId,
                callback = $"{_configuration["Paymob:BaseUrl"]}/api/payment/callback",
                post_url = $"{_configuration["Paymob:BaseUrl"]}/api/payment/server-callback",
                items = new[]
                {
                new
                {
                    name = dto.entityType == EntityType.order ? "Wasla Food Order" : "Wasla Booking",
                    amount = amountCents,
                    description = "Payment via Wasla",
                    quantity = 1
                }
            }
            };

            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/v1/intention/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", _configuration["Paymob:SecretKey"]);
            request.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(request);

            
            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(LocalizationKey.PaymobApiFailed);
            

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var clientSecret = json.RootElement.GetProperty("client_secret").GetString();

            var payment = new Payment
            {
                ResidentId = dto.UserId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = PaymentStatus.Pending,
                TransactionId = null,
                PaymentDate = _dateTimeHelper.Now,
                entityId = dto.entityId,
                entityType = dto.entityType,
                
            };

            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            var redirectUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={_configuration["Paymob:PublicKey"]}&clientSecret={clientSecret}";

            return (payment, redirectUrl);
        }

        public async Task HandlePaymentCallback(string merchantOrderId, bool isSuccess, bool isRefunded, string transactionId)
        {
            var parts = merchantOrderId.Split('_');
            var type = Enum.Parse<EntityType>(parts[0], true);
            var id = int.Parse(parts[1]);

            if (type == EntityType.order)
            {
                if (isRefunded)
                    await UpdateOrderRefunded(id);
                else if (isSuccess)
                    await UpdateOrderSuccess(id, transactionId);
                else
                    await UpdateOrderFailed(id);
            }
            else if (type == EntityType.booking)
            {
                if (isRefunded)
                    await UpdateBookingRefunded(id);
                else if (isSuccess)
                    await UpdateBookingSuccess(id, transactionId);
                else
                    await UpdateBookingFailed(id);
            }
        }

        public async Task<bool> RefundPaymentAsync(RefundDto dto)
        {
            var payment = await _context.Payment
                .FirstOrDefaultAsync(p => p.entityType == dto.entityType && p.entityId == dto.entityId && p.Status == PaymentStatus.Completed);

            if (payment == null)
                throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);

            if (string.IsNullOrEmpty(payment.PaymobTransactionId))
                throw new BadRequestException(LocalizationKey.PaymobApiFailed);

            using var httpClient = new HttpClient();

            var payload = new
            {
                transaction_id = payment.PaymobTransactionId,
                amount_cents = (int)(payment.Amount * 100)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/api/acceptance/void_refund/refund");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", _configuration["Paymob:SecretKey"]);
            request.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(LocalizationKey.RefundFailed);

            if (dto.entityType == EntityType.order)
                await UpdateOrderRefunded(dto.entityId);
            else
                await UpdateBookingRefunded(dto.entityId);

            return true;
        }

        public async Task<Payment> GetPaymentStatusAsync(EntityType entityType, int entityId)
        {
            var payment = await _context.Payment
                .FirstOrDefaultAsync(p => p.entityType == entityType && p.entityId == entityId);

            if (payment == null)
                throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);

            return payment;
        }

        private async Task UpdateOrderSuccess(int orderId, string transactionId)
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
                .FirstOrDefaultAsync(c => c.residentId == order.residentId && c.restaurantId == order.restaurantId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.items);
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateOrderFailed(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId)
                ?? throw new NotFoundException(LocalizationKey.OrderNotFound);

            if (order.paymentStatus == PaymentStatus.Completed)
                return;

            order.paymentStatus = PaymentStatus.Failed;
            order.status = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateOrderRefunded(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId)
                ?? throw new NotFoundException(LocalizationKey.OrderNotFound);

            order.paymentStatus = PaymentStatus.Refunded;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateBookingSuccess(int bookingId, string transactionId)
        {
            var payment = await _context.Payment
                .FirstOrDefaultAsync(p => p.entityType == EntityType.booking && p.entityId == bookingId);

            if (payment == null)
                return;

            payment.Status = PaymentStatus.Completed;
            payment.PaymobTransactionId = transactionId;

            var booking = await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking != null)
                booking.IsPaid = true;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateBookingFailed(int bookingId)
        {
            var payment = await _context.Payment
                .FirstOrDefaultAsync(p => p.entityType == EntityType.booking && p.entityId == bookingId);

            if (payment == null)
                return;

            payment.Status = PaymentStatus.Failed;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateBookingRefunded(int bookingId)
        {
            var payment = await _context.Payment
                .FirstOrDefaultAsync(p => p.entityType == EntityType.booking && p.entityId == bookingId);

            if (payment == null)
                return;

            payment.Status = PaymentStatus.Refunded;

            var booking = await _context.BaseBookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking != null)
                booking.IsPaid = false;

            await _context.SaveChangesAsync();
        }

        private string DetermineIntegrationId(PaymentMethodType paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethodType.Card => _configuration["Paymob:CardIntegrationId"],
                PaymentMethodType.Wallet => _configuration["Paymob:WalletIntegrationId"],
                PaymentMethodType.CashCollection => _configuration["Paymob:CashCollectionIntegrationId"],
                _ => throw new BadRequestException(LocalizationKey.InvalidPaymentMethod)
            };
        }

        public string ComputeHmacSHA512(string data, string secret)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
        }
    }
}