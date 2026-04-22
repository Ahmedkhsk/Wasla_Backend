namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class CheckoutResponse
    {
        public int orderId { get; set; }
        public string? paymentKey { get; set; }
    }
}
