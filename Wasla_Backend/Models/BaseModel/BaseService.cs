namespace Wasla_Backend.Models.BaseModel
{
    public abstract class BaseService
    {
        public int Id { get; set; }
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public ApplicationUser ServiceProvider { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool IsHidden { get; set; } = false;
        public ServiceProviderType Type { get; set; }
    }
}
