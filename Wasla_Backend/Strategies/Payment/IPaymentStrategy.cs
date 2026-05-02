namespace Wasla_Backend.Strategies.Payment
{
    public interface IPaymentStrategy
    {
        Task<PaymentResult> Pay(PaymentContext context);
    }
}
