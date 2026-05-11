using Microsoft.AspNetCore.SignalR;

namespace shkandal_api.Hubs;

public sealed class ClusterPresenceHub : Hub
{
    private readonly ClusterPresenceTracker _tracker;

    public ClusterPresenceHub(ClusterPresenceTracker tracker)
    {
        _tracker = tracker;
    }

    public async Task<int> JoinCluster(string clusterId)
    {
        if (string.IsNullOrWhiteSpace(clusterId))
        {
            return 0;
        }

        var groupName = GetGroupName(clusterId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var (previousClusterId, currentCount) = _tracker.JoinOrMove(Context.ConnectionId, clusterId);

        if (!string.IsNullOrWhiteSpace(previousClusterId) &&
            !string.Equals(previousClusterId, clusterId, StringComparison.OrdinalIgnoreCase))
        {
            await Clients.Group(GetGroupName(previousClusterId))
                .SendAsync("PresenceUpdated", previousClusterId, _tracker.GetCount(previousClusterId));
        }

        await Clients.Group(groupName)
            .SendAsync("PresenceUpdated", clusterId, currentCount);

        return currentCount;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var (clusterId, currentCount) = _tracker.Leave(Context.ConnectionId);

        if (!string.IsNullOrWhiteSpace(clusterId))
        {
            await Clients.Group(GetGroupName(clusterId))
                .SendAsync("PresenceUpdated", clusterId, currentCount);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private static string GetGroupName(string clusterId) => $"cluster:{clusterId}";
}