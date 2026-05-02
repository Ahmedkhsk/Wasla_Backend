namespace Wasla_Backend.Factories.Implementation
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IServiceProvider _provider;

        public PaymentStrategyFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPaymentStrategy Create(PaymentMethodType type)
        {
            return type switch
            {
                PaymentMethodType.Card => _provider.GetRequiredService<PaymobPaymentStrategy>(),
                PaymentMethodType.CashCollection => _provider.GetRequiredService<CashPaymentStrategy>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
