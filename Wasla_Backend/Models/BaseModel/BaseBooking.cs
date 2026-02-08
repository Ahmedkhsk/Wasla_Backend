namespace Wasla_Backend.Models.BaseModel
{
    
    public abstract class BaseBooking
    {
        public int Id { get; set; }
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public ApplicationUser ServiceProvider { get; set; }
        public string ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public ApplicationUser Resident { get; set; }
        public ServiceProviderType Type { get; set; }


    }
}
