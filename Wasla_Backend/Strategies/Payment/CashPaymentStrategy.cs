namespace Wasla_Backend.Strategies.Payment
{
    public class CashPaymentStrategy : IPaymentStrategy
    {
        public Task<PaymentResult> Pay(PaymentContext context)
        {
            return Task.FromResult(new PaymentResult
            {
                status = PaymentStatus.Completed
            });
        }
    }
}
