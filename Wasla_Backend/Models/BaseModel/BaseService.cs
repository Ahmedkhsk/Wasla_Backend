namespace Wasla_Backend.Models.BaseModel
{
    public abstract class BaseService
    {
        public int Id { get; set; }
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public ApplicationUser ServiceProvider { get; set; }
        public ServiceProviderType Type { get; set; }
    }
}
