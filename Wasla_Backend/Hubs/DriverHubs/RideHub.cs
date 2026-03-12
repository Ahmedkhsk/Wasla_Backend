namespace Wasla_Backend.Hubs.DriverHubs
{
   
    public class RideHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("================================");
            Console.WriteLine("SignalR Connection Opened");

            foreach (var claim in Context.User?.Claims ?? new List<Claim>())
            {
                Console.WriteLine($"Claim Type: {claim.Type}  |  Value: {claim.Value}");
            }

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"UserId: {userId}");
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine("================================");

            await base.OnConnectedAsync();
        }
    }
}
