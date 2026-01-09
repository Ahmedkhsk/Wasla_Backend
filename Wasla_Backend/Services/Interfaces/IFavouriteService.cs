namespace Wasla_Backend.Services.Interfaces
{
    public interface IFavouriteService
    {
        public Task<ServiceProviderFavourite> AddFavourite(string residentId, string serviceProviderId);
        public Task RemoveFavourite(int favouriteId);
        public Task<List<ServiceProviderFavourite>> GetAll(string residentId);
        public Task<List<ServiceProviderFavourite>> GetByType(string residentId, ServiceProviderType serviceType);
    }
}
