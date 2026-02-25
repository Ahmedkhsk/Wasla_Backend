namespace Wasla_Backend.DTOs.UserEventDTOS
{
    public class ServiceProviderRsponse
    {
        public List<ServiceProviderEventResponse> serviceProvidersBooking { get; set; }
        public List<ServiceProviderEventResponse> serviceProvidersView { get; set; }
        public List<ServiceProviderEventResponse> serviceProvidersFav { get; set; }
        public List<ServiceProviderConversionResponse> conversion { get; set; }
    }
}
