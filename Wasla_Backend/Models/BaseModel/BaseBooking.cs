namespace Wasla_Backend.Models.BaseModel
{
    
    public abstract class BaseBooking
    {
        public int Id { get; set; }
        public string ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ApplicationUser Resident { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }

    }
}
