namespace Wasla_Backend.Models.GeneralModel
{
    public class Payment
    {
        public int Id { get; set; }
        public string ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ApplicationUser Resident { get; set; }
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public ApplicationUser ServiceProvider { get; set; }
        public BaseService Service { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        public decimal Amount { get; set; }
        public int BookingId { get; set; }
        public DateTime PaymentDate { get; set; } 
        public PaymentMethodType PaymentMethod { get; set; }

        public string TransactionId { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public ServiceProviderType ServiceType { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
