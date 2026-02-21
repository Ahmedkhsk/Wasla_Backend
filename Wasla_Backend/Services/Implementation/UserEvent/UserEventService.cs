namespace Wasla_Backend.Services.Implementation
{
    public class UserEventService : IUserEventService
    {
        private readonly IUserEventRepository _userEventRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;

        public UserEventService(IUserEventRepository userEventRepository, DateTimeHelper dateTimeHelper,IUserRepository userRepository)
        {
            _userEventRepository = userEventRepository;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
        }

        public async Task CreateUserEventAsync(UserEventDto userEventDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userEventDto.userId);
            
            if (user == null)
                throw new NotFoundException("UserNotFound");

            var userEvent = new UserEvent
            {
                userId = userEventDto.userId,
                serviceId = userEventDto.serviceId,
                eventType = userEventDto.eventType,
                timestamp = _dateTimeHelper.Now,
            };

            await _userEventRepository.AddAsync(userEvent);
            await _userEventRepository.SaveChangesAsync();
        }

    }
}
