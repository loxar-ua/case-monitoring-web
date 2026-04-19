using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using ShkandalInfrastructure;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace shkandal_api.Infrastructure;

public static class AlertWebSocketExtensions
{
    public static ConcurrentDictionary<string, WebSocket> MapAlertWebSocket(this WebApplication app)
    {
        var wsClients = new ConcurrentDictionary<string, WebSocket>();

        app.UseWebSockets();

        app.Map("/ws/alerts", async context =>
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("WebSocket request expected.");
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var clientId = Guid.NewGuid().ToString();
            wsClients[clientId] = socket;

            var buffer = new byte[4096];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", CancellationToken.None);
                        break;
                    }

                    // Keep echo behavior for quick websocket testing via wscat.
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
                }
            }
            finally
            {
                wsClients.TryRemove(clientId, out _);
                socket.Dispose();
            }
        });

        return wsClients;
    }

    public static void StartRandomClusterAlerts(
        this WebApplication app,
        ConcurrentDictionary<string, WebSocket> wsClients)
    {
        var appStopping = app.Lifetime.ApplicationStopping;

        _ = Task.Run(async () =>
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            try
            {
                while (await timer.WaitForNextTickAsync(appStopping))
                {
                    using var scope = app.Services.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShkandalDbContext>();

                    var query = dbContext.Set<Cluster>()
                        .AsNoTracking()
                        .Where(c => c.IsRelevant);

                    var totalClusters = await query.CountAsync(appStopping);
                    if (totalClusters == 0)
                    {
                        continue;
                    }

                    var randomIndex = Random.Shared.Next(totalClusters);
                    var randomCluster = await query
                        .OrderBy(c => c.Id)
                        .Skip(randomIndex)
                        .Select(c => new { c.Id, c.Name })
                        .FirstOrDefaultAsync(appStopping);

                    if (randomCluster is null)
                    {
                        continue;
                    }

                    var payload = JsonSerializer.Serialize(new
                    {
                        title = randomCluster.Name,
                        message = "Кластер оновився.",
                        clusterId = randomCluster.Id,
                        updatedAt = DateTime.UtcNow
                    });

                    await BroadcastPayloadAsync(payload, wsClients, appStopping);
                }
            }
            catch (OperationCanceledException)
            {
                // App is shutting down.
            }
        }, appStopping);
    }

    private static async Task<int> BroadcastPayloadAsync(
        string payload,
        ConcurrentDictionary<string, WebSocket> wsClients,
        CancellationToken cancellationToken)
    {
        var bytes = Encoding.UTF8.GetBytes(payload);
        var sentTo = 0;

        foreach (var (clientId, socket) in wsClients)
        {
            if (socket.State != WebSocketState.Open)
            {
                wsClients.TryRemove(clientId, out _);
                continue;
            }

            try
            {
                await socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
                sentTo += 1;
            }
            catch
            {
                wsClients.TryRemove(clientId, out _);
            }
        }

        return sentTo;
    }
}
