namespace Wasla_Backend.Services.Implementation
{
    public class UserEventService : IUserEventService
    {
        private readonly IUserEventRepository _userEventRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserEventService(IUserEventRepository userEventRepository, 
                                DateTimeHelper dateTimeHelper,
                                IUserRepository userRepository,
                                IMapper mapper)
        {
            _userEventRepository = userEventRepository;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserEventAsync(UserEventDto userEventDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userEventDto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var serviceProvider = await _userRepository.GetUserByIdAsync(userEventDto.serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            var userEvent = _mapper.Map<UserEvent>(userEventDto);
            userEvent.timestamp = _dateTimeHelper.Now;
           

            await _userEventRepository.AddAsync(userEvent);
            await _userEventRepository.SaveChangesAsync();
        }

        public async Task<List<ServiceProviderEventResponse>> GetTopServiceProvidersAsync(string userId, int top)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            return  await _userEventRepository.GetTopServiceProvidersAsync(userId, top);
        }

    }
}
