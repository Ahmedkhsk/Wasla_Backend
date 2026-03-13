namespace Wasla_Backend.Services.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public FavouriteService(
            IFavouriteRepository favouriteRepository,
            IUserRepository userRepository,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<ServiceProviderFavourite> AddFavourite(string residentId, string serviceProviderId, string lan)
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

            var metadata = new Dictionary<string, string>
            {
                { "UserName", user.FullName ?? string.Empty }
            };

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    serviceProviderId,
                    NotificationType.allFavouritesScreen,
                    serviceProviderId,
                    _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
                    lan,
                    metadata
                ));

            return new ServiceProviderFavourite
            {
                id = favourite.Id,
                residentId = residentId,
                serviceProviderId = serviceProviderId,
                serviceProviderName = serviceProvider.FullName,
                serviceProviderProfilePhoto = _fileUrlBuilderService.GetMediaUrl(serviceProvider.ProfilePhoto, MediaType.userImage),
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