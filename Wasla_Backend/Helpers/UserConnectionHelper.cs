public class UserConnectionHelper
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _connections = new();

    public void AddConnection(string userId, string connectionId)
    {
        var userConnections = _connections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
        userConnections.TryAdd(connectionId, 0);
    }

    public void RemoveConnection(string userId, string connectionId)
    {
        if (!_connections.TryGetValue(userId, out var userConnections))
            return;

        userConnections.TryRemove(connectionId, out _);

        if (userConnections.IsEmpty)
            _connections.TryRemove(userId, out _);
    }

    public int GetConnectionCount(string userId)
    {
        return _connections.TryGetValue(userId, out var userConnections)
            ? userConnections.Count
            : 0;
    }
}