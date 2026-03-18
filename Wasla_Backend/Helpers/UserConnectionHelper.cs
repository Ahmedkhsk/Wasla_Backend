namespace Wasla_Backend.Helpers
{
    public class UserConnectionHelper
    {
        private readonly Dictionary<string, HashSet<string>> _connections = new();

        public void AddConnection(string userId, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.ContainsKey(userId))
                    _connections[userId] = new HashSet<string>();

                _connections[userId].Add(connectionId);
            }
        }

        public void RemoveConnection(string userId, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.ContainsKey(userId))
                    return;

                _connections[userId].Remove(connectionId);

                if (_connections[userId].Count == 0)
                    _connections.Remove(userId);
            }
        }

        public int GetConnectionCount(string userId)
        {
            lock (_connections)
            {
                return _connections.ContainsKey(userId)
                    ? _connections[userId].Count
                    : 0;
            }
        }
    }
}
