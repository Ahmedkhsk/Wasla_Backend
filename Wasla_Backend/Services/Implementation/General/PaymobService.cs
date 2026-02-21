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

        public async Task<(Payment Payment, string RedirectUrl)> ProcessPaymentAsync(CreatePaymentDto createPaymentDto)
        {
            if (string.IsNullOrEmpty(createPaymentDto.UserId))
                throw new BadRequestException(LocalizationKey.ResidentIdRequired);

            var resident = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == createPaymentDto.UserId);

            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            if (string.IsNullOrEmpty(createPaymentDto.ServiceProviderId))
                throw new BadRequestException(LocalizationKey.ServiceProviderIdRequired);

            var serviceProvider = await _context.Users
               .FirstOrDefaultAsync(u => u.Id == createPaymentDto.ServiceProviderId);

            if (serviceProvider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            if (createPaymentDto.Amount <= 0)
                throw new BadRequestException(LocalizationKey.AmountMustBeGreaterThanZero);

            if (createPaymentDto.ServiceId <= 0)
                throw new BadRequestException(LocalizationKey.ServiceIdRequired);

            var service = await _context.BaseServices
              .FirstOrDefaultAsync(s => s.Id == createPaymentDto.ServiceId);

            if (service == null)
                throw new NotFoundException(LocalizationKey.ServiceNotFound);

            using var httpClient = new HttpClient();

            string secretKey = _configuration["Paymob:SecretKey"];
            string publicKey = _configuration["Paymob:PublicKey"];

            int specialReference = RandomNumberGenerator.GetInt32(1000000, 9999999);
            var amountCents = (int)(createPaymentDto.Amount * 100);

            var names = (resident.FullName ?? "Guest User").Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
                payment_methods = new[] { int.Parse(DetermineIntegrationId(createPaymentDto.PaymentMethod)) },
                billing_data = billingData,
                customer = new { id = createPaymentDto.UserId },
                special_reference = specialReference,
                expiration = 3600,
                merchant_order_id = specialReference.ToString(),
                callback = "https://your-domain.com/api/payment/callback",
                post_url = "https://your-domain.com/api/payment/server-callback"
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymob.com/v1/intention/");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Token", secretKey);
            requestMessage.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseContent);
                throw new BadRequestException(LocalizationKey.PaymobApiFailed);
            }

            var resultJson = JsonDocument.Parse(responseContent);
            var clientSecret = resultJson.RootElement.GetProperty("client_secret").GetString();

            var payment = new Payment
            {
                ResidentId = createPaymentDto.UserId,
                Resident = resident,
                ServiceProviderId = createPaymentDto.ServiceProviderId,
                ServiceId = createPaymentDto.ServiceId,
                Amount = createPaymentDto.Amount,
                PaymentMethod = createPaymentDto.PaymentMethod,
                Status = PaymentStatus.Pending,
                TransactionId = specialReference.ToString(),
                PaymentDate = _dateTimeHelper.Now,
                ServiceType = createPaymentDto.ServiceProviderType
            };

            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            string redirectUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={publicKey}&clientSecret={clientSecret}";

            return (payment, redirectUrl);
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

        public async Task<Payment> UpdateOrderSuccess(string transactionId)
        {
            var payment = await _context.Payment
                .Include(p => p.Resident)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (payment == null)
                throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);

            payment.Status = PaymentStatus.Completed;
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> UpdateOrderFailed(string transactionId)
        {
            var payment = await _context.Payment
                .Include(p => p.Resident)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (payment == null)
                throw new NotFoundException(LocalizationKey.PaymentMethodNotFound);

            payment.Status = PaymentStatus.Failed;
            await _context.SaveChangesAsync();

            return payment;
        }

        public string ComputeHmacSHA512(string data, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using var hmac = new HMACSHA512(keyBytes);
            var hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}