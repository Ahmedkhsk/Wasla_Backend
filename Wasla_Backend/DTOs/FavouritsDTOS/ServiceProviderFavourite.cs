namespace Wasla_Backend.DTOs.FavouritsDTOS
{
    public class ServiceProviderFavourite
    {
        public int id { get; set; }
        public string serviceProviderName { get; set; }
        public string serviceProviderProfilePhoto { get; set; }
        public string serviceProviderPhone { get; set; }
        public ServiceProviderType ServiceProviderType { get; set; }


    }
}
