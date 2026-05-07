namespace Wasla_Backend.Models.BaseModel
{
    
    public abstract class BaseBooking
    {
        public int Id { get; set; }
        public string ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ApplicationUser Resident { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }
        public DateTime Date { get; set; }
        public BaseBookingStatus baseBookingStatus { get; set; } = BaseBookingStatus.Pending;


        public bool IsPaid { get; set; } = false;
        public double price { get; set; }


    }
}
