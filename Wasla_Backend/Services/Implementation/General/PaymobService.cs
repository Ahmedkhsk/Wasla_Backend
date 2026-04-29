using Wasla_Backend.DTOs.PaymentDtos;

namespace Wasla_Backend.Services.Implementation.General
{
    public class PaymobService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository _paymentRepository;
        private readonly DateTimeHelper _dateTimeHelper;

        public PaymobService(
            IConfiguration configuration,
            IPaymentRepository paymentRepository,
            DateTimeHelper dateTimeHelper)
        {
            _configuration = configuration;
            _paymentRepository = paymentRepository;
            _dateTimeHelper = dateTimeHelper;
        }


        public async Task<(Payment payment, string redirectUrl)> ProcessPaymentAsync(CreatePaymentDto dto)
        {
            ValidatePaymentDto(dto);

            var resident = await _paymentRepository.GetUserByIdAsync(dto.UserId)
                ?? throw new NotFoundException(LocalizationKey.ResidentNotFound);

            await ValidateEntityExistsAsync(dto.entityType, dto.entityId);

            var amountCents = (int)(dto.Amount * 100);
            var billingData = BuildBillingData(resident);
            var payload = BuildPaymobPayload(dto, amountCents, billingData);
            var intentionRes = await SendToPaymobAsync(payload);

            var payment = new Payment
            {
                ResidentId = dto.UserId,
                ServiceProviderId=dto.ServiceProviderId,
                ServiceType = dto.ServiceType,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = PaymentStatus.Pending,
                PaymobOrderId = intentionRes.intentionOrderId,
                TransactionId = null,
                PaymentDate = _dateTimeHelper.Now,
                entityId = dto.entityId,
                entityType = dto.entityType,
            };

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            var redirectUrl = BuildRedirectUrl(intentionRes.clientSecret);

            return (payment, redirectUrl);
        }


        public async Task HandlePaymentCallbackByPaymobOrderId(
            string paymobOrderId,
            bool isSuccess,
            bool isRefunded,
            string transactionId)
        {
            var payment = await _paymentRepository.GetByPaymobOrderIdAsync(paymobOrderId);
            if (payment == null || payment.Status == PaymentStatus.Completed)
                return;

            await UpdateEntityStatusAsync(payment.entityType, payment.entityId, isSuccess, isRefunded, transactionId);

            payment.Status = isRefunded ? PaymentStatus.Refunded
                                  : isSuccess ? PaymentStatus.Completed
                                               : PaymentStatus.Failed;
            payment.TransactionId = transactionId;
            payment.PaymobTransactionId = transactionId;


            await _paymentRepository.SaveChangesAsync();
        }


        public async Task<bool> RefundPaymentAsync(EntityTypeDto dto)
        {
            var payment = await _paymentRepository.GetByEntityAsync(dto.entityType, dto.entityId, PaymentStatus.Completed)
                ?? throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);

            if (string.IsNullOrEmpty(payment.PaymobTransactionId))
                throw new BadRequestException(LocalizationKey.PaymobApiFailed);

            await SendRefundRequestAsync(payment.PaymobTransactionId, (int)(payment.Amount * 100));

            await UpdateEntityOnRefundAsync(dto.entityType, dto.entityId);

            return true;
        }


        public async Task<PaymentStatusResponse> GetPaymentStatusAsync(EntityType entityType, int entityId)
            => await _paymentRepository.GetPaymentStatusAsync(entityType, entityId)
               ?? throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);


        public async Task<List<UserPaymentDto>> GetAllPaymentsAsync(string residentId)
            => await _paymentRepository.GetAllPaymentsByResidentAsync(residentId);

     

        private void ValidatePaymentDto(CreatePaymentDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserId))
                throw new BadRequestException(LocalizationKey.ResidentIdRequired);

            if (dto.Amount <= 0)
                throw new BadRequestException(LocalizationKey.AmountMustBeGreaterThanZero);
        }

        private async Task ValidateEntityExistsAsync(EntityType entityType, int entityId)
        {
            if (entityType == EntityType.booking)
            {
                _ = await _paymentRepository.GetBookingByIdAsync(entityId)
                    ?? throw new NotFoundException(LocalizationKey.BookingNotFound);
            }
            else if (entityType == EntityType.order)
            {
                _ = await _paymentRepository.GetOrderByIdAsync(entityId)
                    ?? throw new NotFoundException(LocalizationKey.OrderNotFound);
            }
        }

        private object BuildBillingData(ApplicationUser resident)
        {
            var names = (resident.FullName ?? "Guest User").Split(' ');
            var firstName = names.Length > 0 ? names[0] : "Guest";
            var lastName = names.Length > 1 ? names[^1] : "User";

            return new
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
        }

        private object BuildPaymobPayload(CreatePaymentDto dto, int amountCents, object billingData)
            => new
            {
                amount = amountCents,
                currency = "EGP",
                payment_methods = new[] { int.Parse(GetIntegrationId(dto.PaymentMethod)) },
                billing_data = billingData,
                customer = new { id = dto.UserId },
                expiration = 3600,
                callback = $"{_configuration["Paymob:BaseUrl"]}/api/payment/callback",
                post_url = $"{_configuration["Paymob:BaseUrl"]}/api/payment/server-callback",
                items = new[]
                {
                    new
                    {
                        name        = dto.entityType == EntityType.order ? "Wasla Order" : "Wasla Booking",
                        amount      = amountCents,
                        description = "Payment via Wasla",
                        quantity    = 1
                    }
                }
            };

        private async Task<(string clientSecret, string intentionOrderId)> SendToPaymobAsync(object payload)
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/v1/intention/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", _configuration["Paymob:SecretKey"]);
            request.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(LocalizationKey.PaymobApiFailed);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var clientSecret = json.RootElement.GetProperty("client_secret").GetString()!;
            var intentionId = json.RootElement.GetProperty("intention_order_id").GetInt64().ToString();

            return (clientSecret, intentionId);
        }

        private async Task SendRefundRequestAsync(string paymobTransactionId, int amountCents)
        {
            using var httpClient = new HttpClient();
            var payload = new { transaction_id = paymobTransactionId, amount_cents = amountCents };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/api/acceptance/void_refund/refund");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", _configuration["Paymob:SecretKey"]);
            request.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
               Console.WriteLine($"Paymob Error: {responseBody}");
            }
        }

        private string BuildRedirectUrl(string clientSecret)
            => $"https://accept.paymob.com/unifiedcheckout/?publicKey={_configuration["Paymob:PublicKey"]}&clientSecret={clientSecret}";

        private async Task UpdateEntityStatusAsync(
            EntityType entityType,
            int entityId,
            bool isSuccess,
            bool isRefunded,
            string transactionId)
        {
            if (entityType == EntityType.order)
            {
                if (isRefunded) await _paymentRepository.UpdateOrderOnRefundAsync(entityId);
                else if (isSuccess) await _paymentRepository.UpdateOrderOnSuccessAsync(entityId, transactionId);
                else await _paymentRepository.UpdateOrderOnFailureAsync(entityId);
            }
            else
            {
                if (isRefunded) await _paymentRepository.UpdateBookingOnRefundAsync(entityId);
                else if (isSuccess) await _paymentRepository.UpdateBookingOnSuccessAsync(entityId, transactionId);
                else await _paymentRepository.UpdateBookingOnFailureAsync(entityId);
            }
        }

        private async Task UpdateEntityOnRefundAsync(EntityType entityType, int entityId)
        {
            if (entityType == EntityType.order)
                await _paymentRepository.UpdateOrderOnRefundAsync(entityId);
            else
                await _paymentRepository.UpdateBookingOnRefundAsync(entityId);
        }

        private string GetIntegrationId(PaymentMethodType paymentMethod)
            => paymentMethod switch
            {
                PaymentMethodType.Card => _configuration["Paymob:CardIntegrationId"],
                PaymentMethodType.Wallet => _configuration["Paymob:WalletIntegrationId"],
                PaymentMethodType.CashCollection => _configuration["Paymob:CashCollectionIntegrationId"],
                _ => throw new BadRequestException(LocalizationKey.InvalidPaymentMethod)
            };
    }
}