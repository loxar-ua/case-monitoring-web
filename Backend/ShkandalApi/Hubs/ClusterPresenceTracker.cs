namespace shkandal_api.Hubs;

public sealed class ClusterPresenceTracker
{
    private readonly object _sync = new();
    private readonly Dictionary<string, string> _connectionClusters = new();
    private readonly Dictionary<string, int> _clusterCounts = new(StringComparer.OrdinalIgnoreCase);

    public (string? PreviousClusterId, int CurrentCount) JoinOrMove(string connectionId, string clusterId)
    {
        lock (_sync)
        {
            if (_connectionClusters.TryGetValue(connectionId, out var currentClusterId))
            {
                if (string.Equals(currentClusterId, clusterId, StringComparison.OrdinalIgnoreCase))
                {
                    return (currentClusterId, GetCountNoLock(clusterId));
                }

                DecrementNoLock(currentClusterId);
            }

            _connectionClusters[connectionId] = clusterId;

            var count = IncrementNoLock(clusterId);
            return (currentClusterId, count);
        }
    }

    public (string? ClusterId, int CurrentCount) Leave(string connectionId)
    {
        lock (_sync)
        {
            if (!_connectionClusters.TryGetValue(connectionId, out var clusterId))
            {
                return (null, 0);
            }

            _connectionClusters.Remove(connectionId);
            var count = DecrementNoLock(clusterId);
            return (clusterId, count);
        }
    }

    public int GetCount(string clusterId)
    {
        lock (_sync)
        {
            return GetCountNoLock(clusterId);
        }
    }

    private int IncrementNoLock(string clusterId)
    {
        var currentCount = GetCountNoLock(clusterId) + 1;
        _clusterCounts[clusterId] = currentCount;
        return currentCount;
    }

    private int DecrementNoLock(string clusterId)
    {
        var currentCount = GetCountNoLock(clusterId);
        if (currentCount <= 1)
        {
            _clusterCounts.Remove(clusterId);
            return 0;
        }

        var nextCount = currentCount - 1;
        _clusterCounts[clusterId] = nextCount;
        return nextCount;
    }

    private int GetCountNoLock(string clusterId)
        => _clusterCounts.TryGetValue(clusterId, out var currentCount) ? currentCount : 0;
}