namespace Wasla_Backend.Models
{
    public class ApplicationRole : IdentityRole
    {
        public MultilingualText RoleName { get; set; }
    }
}
