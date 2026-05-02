namespace Wasla_Backend.Strategies.Payment
{
    public class PaymobPaymentStrategy : IPaymentStrategy
    {
        private readonly IPaymentService _paymentService;

        public PaymobPaymentStrategy(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<PaymentResult> Pay(PaymentContext context)
        {
            var (_, paymentUrl) = await _paymentService.ProcessPaymentAsync(new CreatePaymentDto
            {
                Amount = context.Amount,
                entityId = context.OrderId,
                entityType = EntityType.order,
                UserId = context.UserId,
                ServiceProviderId = context.ServiceProviderId,
                ServiceType = ServiceProviderType.Restaurant,
                PaymentMethod = PaymentMethodType.Card
            });

            return new PaymentResult
            {
                paymentUrl = paymentUrl,
                status = PaymentStatus.Pending
            };
        }
    }
}
