using Notification = FirebaseAdmin.Messaging.Notification;

namespace Wasla_Backend.Services.Implementation.General
{
    public class FirebaseService : IFirebaseService
    {
        public FirebaseApp App { get; private set; }

        public FirebaseService(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseSection = configuration.GetSection("Firebase");
                var firebaseConfig = firebaseSection.Get<FireBaseSettings>();
                firebaseConfig.Private_key = firebaseConfig.Private_key.Replace("\\n", "\n");
                var json = JsonSerializer.Serialize(firebaseConfig);

                App = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(json)
                });
            }
            else
            {
                App = FirebaseApp.DefaultInstance;
            }
        }

        public async Task SubscribeDeviceAsync(string deviceToken, string userId)
        {
            var userTopic = $"User_{userId}";
            var allTopic = "All";
            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new List<string> { deviceToken }, userTopic);
            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new List<string> { deviceToken }, allTopic);
        }

        public async Task UnsubscribeDeviceAsync(string deviceToken, string userId)
        {
            var userTopic = $"User_{userId}";
            var allTopic = "All";
            await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(new List<string> { deviceToken }, userTopic);
            await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(new List<string> { deviceToken }, allTopic);
        }

        public async Task<string> SendToTopicAsync(string topic, string title, string body,string refrenceId,NotificationType type)
        {
            var data = new Dictionary<string, string>();
            data["refrenceId"] = refrenceId;
            data["type"] = type.ToString();
            
            var message = new Message { Topic = topic, Notification = new Notification { Title = title, Body = body },Data=data };
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<string> SendToDeviceAsync(string deviceToken, string title, string body, string refrenceId, NotificationType type)
        {
            var data = new Dictionary<string, string>();
            data["refrenceId"] = refrenceId;
            data["type"] = type.ToString();
            var message = new Message { Token = deviceToken, Notification = new Notification { Title = title, Body = body },Data=data };
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
