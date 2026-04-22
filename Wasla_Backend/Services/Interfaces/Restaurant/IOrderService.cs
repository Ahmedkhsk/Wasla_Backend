namespace Wasla_Backend.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CheckoutResponse> Checkout(CheckoutDto dto);
    }
}
