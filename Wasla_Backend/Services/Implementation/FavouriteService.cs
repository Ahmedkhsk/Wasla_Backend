

namespace Wasla_Backend.Services.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;
        public FavouriteService(IFavouriteRepository favouriteRepository,IUserRepository userRepository)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
        }
        public async Task AddFavourite(string residentId, string serviceProviderId)
        {
            var user = await _userRepository.GetUserByIdAsync(residentId);
            if(user == null) 
                throw new NotFoundException("UserNotFound");
            var serviceProvider=await _userRepository.GetUserByIdAsync(serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException("ServiceProviderNotFound");
            var favourite = new Favourites
            {
                UserId = residentId,
                ServiceProviderId = serviceProviderId,
                ServiceType = ServiceProviderTypeResolver.Resolve(serviceProvider)
            };
            await _favouriteRepository.AddAsync(favourite);
           await _favouriteRepository.SaveChangesAsync();
        }

        public async Task<List<ServiceProviderFavourite>> GetAll(string residentId)
        {
            var resident=await _userRepository.GetUserByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException("UserNotFound");
            return await _favouriteRepository.GetAllAsync(residentId);
        }

        public async Task<List<ServiceProviderFavourite>> GetByType(string residentId, ServiceProviderType serviceType)
        {
           var resident = _userRepository.GetUserByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException("UserNotFound");
            return await _favouriteRepository.GetByTypeAsync(residentId, serviceType);
        }

        public async Task RemoveFavourite(int favouriteId)
        {
           var favourite =await _favouriteRepository.GetByIdAsync(favouriteId);
            if (favourite == null)
                throw new NotFoundException("FavouriteNotFound");
            _favouriteRepository.Delete(favourite);
           await _favouriteRepository.SaveChangesAsync();
        }

    }
}
