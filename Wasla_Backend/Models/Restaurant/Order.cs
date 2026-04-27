namespace Wasla_Backend.Models.Restaurant
{
    public class Order  
    {
        public int id { get; set; }

        [ForeignKey("resident")]
        public string residentId { get; set; }
        public Resident resident { get; set; }

        [ForeignKey("restaurant")]
        public string restaurantId { get; set; }
        public Restaurant restaurant { get; set; }

        public decimal totalPrice { get; set; }
        public string? address { get; set; }
        public string? notes { get; set; }
        public decimal deliveryFee { get; set; }
        public string? paymobOrderId { get; set; }
        public string? paymentKey { get; set; }
        public string? transactionId { get; set; }
        public OrderStatus status { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public PaymentMethodType paymentMethod { get; set; }
        public DateTime createdAt { get; set; }
        public ICollection<OrderItem>? items { get; set; }
    }
}
