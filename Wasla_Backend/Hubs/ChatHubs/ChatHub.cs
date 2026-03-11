namespace Wasla_Backend.Hubs.ChatHubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            await Clients.All.SendAsync("UserOnline", userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            await Clients.All.SendAsync("UserOffline", userId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Typing(string receiverId)
        {
            var senderId = Context.UserIdentifier;

            await Clients.User(receiverId)
                .SendAsync("UserTyping", senderId);
        }

        public async Task StopTyping(string receiverId)
        {
            var senderId = Context.UserIdentifier;

            await Clients.User(receiverId)
                .SendAsync("UserStopTyping", senderId);
        }
    }
}