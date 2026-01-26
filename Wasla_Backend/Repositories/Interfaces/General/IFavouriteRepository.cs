namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IFavouriteRepository : IGenericRepository<Favourites>
    {
        public Task<List<ServiceProviderFavourite>> GetAllAsync(string residentId);

         public Task<List<ServiceProviderFavourite>> GetByTypeAsync(string residentId, ServiceProviderType serviceType);
    }
}
