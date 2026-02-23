namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IFirebaseService
    {
        Task SubscribeDeviceAsync(string deviceToken, string userId);
        Task UnsubscribeDeviceAsync(string deviceToken, string userId);
        Task<string> SendToTopicAsync(string topic, string title, string body);
        Task<string> SendToDeviceAsync(string deviceToken, string title, string body);
    }
}
