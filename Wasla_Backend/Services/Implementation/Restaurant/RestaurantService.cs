namespace Wasla_Backend.Services.Implementation
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IGenericRepository<RestaurantCategory> _restaurantCategoryRepo;
        private readonly IReservationRepository _reservationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public RestaurantService
            (
            IRestaurantRepository restaurantRepository, IUserRepository userRepository,
            IFileService fileService, IMapper mapper, IFileUrlBuilderService fileUrlBuilderService,
            IGenericRepository<RestaurantCategory> restaurantCategoryRepo,
            IReservationRepository reservationRepository, IOrderRepository orderRepository,
            IUserAuthorizationService userAuthorizationService
            )
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _fileService = fileService;
            _mapper = mapper;
            _fileUrlBuilderService = fileUrlBuilderService;
            _restaurantCategoryRepo = restaurantCategoryRepo;
            _reservationRepository = reservationRepository;
            _orderRepository = orderRepository;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task CompleteProfile(CompleteRegisterRestaurantDto dto)
        {
            var restaurant = await _restaurantRepository.GetByEmailAsync(dto.email);

            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var category = await _restaurantCategoryRepo.GetByIdAsync(dto.restaurantCategoryId);
            if (category == null)
                throw new NotFoundException(LocalizationKey.RestaurantCategoryNotFound);

            _mapper.Map(dto, restaurant);

            if (dto.profile != null)
            {
                restaurant.ProfilePhoto = await _fileService.AddFileAsync(
                            dto.profile,
                            _fileUrlBuilderService.GetPath(MediaType.userImage));
            }

            if (dto.gallery != null)
            {
                restaurant.images = await _fileService.AddFilesAsync(
                            dto.gallery,
                            _fileUrlBuilderService.GetPath(MediaType.restaurantImage));
            }

            restaurant.IsCompleteRegistration = true;
            _restaurantRepository.Update(restaurant);
            await _restaurantRepository.SaveChangesAsync();
        }

        public async Task UpdateRestaurant(UpdateRestaurantDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(dto.id);

            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(restaurant.Id);

            var category = await _restaurantCategoryRepo.GetByIdAsync(dto.restaurantCategoryId);
            if (category == null)
                throw new NotFoundException(LocalizationKey.RestaurantCategoryNotFound);

            var existingFileNames = _fileService.ExtractFileNames(dto.files.existingFiles);

            _mapper.Map(dto, restaurant);

            if (dto.profile != null)
            {
                restaurant.ProfilePhoto = await _fileService.ReplaceFileAsync(
                            restaurant.ProfilePhoto,
                            dto.profile,
                            _fileUrlBuilderService.GetPath(MediaType.userImage));
            }

            if (dto.files.newFiles != null)
            {
                restaurant.images = await _fileService.ReplaceFilesAsync(
                            restaurant.images,
                            existingFileNames,
                            dto.files.newFiles,
                            _fileUrlBuilderService.GetPath(MediaType.restaurantImage));
            }

            _restaurantRepository.Update(restaurant);
            await _restaurantRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<GetAllRestaurantsResponse>> GetAll(GetGeneralWithPaginationDto<int> paginationParams)
        {
            var result = await _restaurantRepository.GetAllRestaurants(paginationParams);

            var mappedItems = result.Data.Select(r =>
            {
                var mapped = _mapper.Map<GetAllRestaurantsResponse>(r);

                mapped.profile = _fileUrlBuilderService.GetMediaUrl(
                    r.ProfilePhoto,
                    MediaType.userImage
                );

                mapped.gallery = r.images.Select(image => _fileUrlBuilderService.GetMediaUrl(
                    image,
                    MediaType.restaurantImage
                )).ToList();

                return mapped;
            }).ToList();

            return new PagedResult<GetAllRestaurantsResponse>
            {
                Data = mappedItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<GetRestaurantResponse> GetRestaurant(GetGeneralDto<string> dto)
        {
            var restaurant = await _restaurantRepository.GetByUserIdAsync(dto.id);
            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(restaurant.Id);

            var numberofOrders = await _orderRepository.CountOrders(restaurant.Id, OrderStatus.Delivered);

            var mapped = _mapper.Map<GetRestaurantResponse>(restaurant, opt =>
            {
                opt.Items["lang"] = dto.lan;
            });

            mapped.numberOfCompletedOrders = numberofOrders;
            mapped.profile = _fileUrlBuilderService.GetMediaUrl(
                restaurant.ProfilePhoto,
                MediaType.userImage
            );
            mapped.gallery = restaurant.images.Select(image => _fileUrlBuilderService.GetMediaUrl(
                image,
                MediaType.restaurantImage
            )).ToList();
            return mapped;
        }
        
        public async Task<RestaurantCharts> GetCharts(string restaurantId)
        {
            var user = await _restaurantRepository.GetByUserIdAsync(restaurantId);

            if(user==null)
                throw new NotFoundException(LocalizationKey.RestaurantNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(user.Id);

            return new RestaurantCharts
            {
                numOfOrders = await _orderRepository.CountOrders(user.Id, null),
                numOfCompletedOrders = await _orderRepository.CountOrders(user.Id, OrderStatus.Delivered),
                numberOfReservations = await _reservationRepository.CountReservations(user.Id),
                totalAmount = (decimal)await _orderRepository.TotalAmountOfOrders(user.Id),
                years = await _orderRepository.GetCollectedPriceByYear(user.Id)
            };
        }
    }
}
