namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class OrderResponse
    {
        public int id { get; set; }
        public string residentId { get; set; }
        public string restaurantId { get; set; }
        public string residentName { get; set; }
        public string restaurantName { get; set; }
        public decimal totalPrice { get; set; }
        public string address { get; set; }
        public string? notes { get; set; }
        public decimal deliveryFee { get; set; }
        public string paymobOrderId { get; set; }
        public string paymentKey { get; set; }
        public string transactionId { get; set; }
        public OrderStatus status { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public PaymentMethodType paymentMethod { get; set; }
        public DateTime createdAt { get; set; }
        public List<OrderItemsResponse> items { get; set; }
    }
}
