namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class CheckoutDto
    {
        public string restaurantId { get; set; }
        public string residentId { get; set; }
        public string address { get; set; }
        public string? notes { get; set; }
        public PaymentMethodType paymentMethod { get; set; }

    }
}
