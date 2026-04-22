namespace Wasla_Backend.DTOs
{
    public class PaymentStatusResponse
    {
        public bool isPaid { get; set; }

        public string status { get; set; }

        public string paymentMethod { get; set; }

        public decimal amount { get; set; }

        public string paymobTransactionId { get; set; }

    }
}
