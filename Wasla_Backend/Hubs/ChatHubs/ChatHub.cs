public class ChatHub : Hub
{
    private readonly UserConnectionHelper _connectionManager;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeHelper _dateTimeHelper;

    public ChatHub(
        UserConnectionHelper connectionManager,
        IUserRepository userRepository,
        DateTimeHelper dateTimeHelper)
    {
        _connectionManager = connectionManager;
        _userRepository = userRepository;
        _dateTimeHelper = dateTimeHelper;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;

        if (!string.IsNullOrEmpty(userId))
        {
            _connectionManager.AddConnection(userId, connectionId);

            if (_connectionManager.GetConnectionCount(userId) == 1)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    user.isOnline = true;
                    await _userRepository.UpdateUserAsync(user);
                    await Clients.All.SendAsync("UserOnline", new { userId });
                }
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;

        if (!string.IsNullOrEmpty(userId))
        {
            _connectionManager.RemoveConnection(userId, connectionId);

            if (_connectionManager.GetConnectionCount(userId) == 0)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    user.isOnline = false;
                    user.lastSeen = _dateTimeHelper.Now;
                    await _userRepository.UpdateUserAsync(user);
                    await Clients.All.SendAsync("UserOffline", new { userId, lastSeen = user.lastSeen });
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task Typing(string receiverId)
    {
        var senderId = Context.UserIdentifier;
        await Clients.User(receiverId).SendAsync("UserTyping", new { senderId });
    }

    public async Task StopTyping(string receiverId)
    {
        var senderId = Context.UserIdentifier;
        await Clients.User(receiverId).SendAsync("UserStopTyping", new { senderId });
    }
}