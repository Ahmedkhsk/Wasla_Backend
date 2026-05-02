namespace Wasla_Backend.Factories.Interfaces
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy Create(PaymentMethodType type);
    }
}
