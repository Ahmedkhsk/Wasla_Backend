namespace Wasla_Backend.Services.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;

        public FavouriteService(IFavouriteRepository favouriteRepository, IUserRepository userRepository)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceProviderFavourite> AddFavourite(string residentId, string serviceProviderId)
        {
            var user = await _userRepository.GetUserByIdAsync(residentId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var serviceProvider = await _userRepository.GetUserByIdAsync(serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            var favourite = new Favourites
            {
                UserId = residentId,
                ServiceProviderId = serviceProviderId,
                ServiceType = ServiceProviderTypeResolver.Resolve(serviceProvider)
            };

            await _favouriteRepository.AddAsync(favourite);
            await _favouriteRepository.SaveChangesAsync();

            return new ServiceProviderFavourite
            {
                id = favourite.Id,
                residentId = residentId,
                serviceProviderId = serviceProviderId,
                serviceProviderName = serviceProvider.FullName,
                serviceProviderProfilePhoto = serviceProvider.ProfilePhoto,
                serviceProviderPhone = serviceProvider.Phone,
                ServiceProviderType = favourite.ServiceType.ToString()
            };
        }

        public async Task<List<ServiceProviderFavourite>> GetAll(string residentId)
        {
            var resident = await _userRepository.GetUserByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            return await _favouriteRepository.GetAllAsync(residentId);
        }

        public async Task<List<ServiceProviderFavourite>> GetByType(string residentId, ServiceProviderType serviceType)
        {
            var resident = await _userRepository.GetUserByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            return await _favouriteRepository.GetByTypeAsync(residentId, serviceType);
        }

        public async Task RemoveFavourite(int favouriteId)
        {
            var favourite = await _favouriteRepository.GetByIdAsync(favouriteId);
            if (favourite == null)
                throw new NotFoundException(LocalizationKey.FavouriteNotFound);

            _favouriteRepository.Delete(favourite);
            await _favouriteRepository.SaveChangesAsync();
        }
    }
}