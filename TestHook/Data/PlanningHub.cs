using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TestHook.Data
{
    public class PlanningHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> Subscribers = new();

        public override Task OnConnectedAsync()
        {
            Subscribers.TryAdd(Context.ConnectionId, Context.ConnectionId);
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Subscribers.TryRemove(Context.ConnectionId, out _);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Subscribe()
        {
            Subscribers.TryAdd(Context.ConnectionId, Context.ConnectionId);
            Console.WriteLine($"Client subscribed: {Context.ConnectionId}");
        }

        public async Task Unsubscribe()
        {
            Subscribers.TryRemove(Context.ConnectionId, out _);
            Console.WriteLine($"Client unsubscribed: {Context.ConnectionId}");
        }

        public async Task SendPlanningData(IEnumerable<SimpleDataForHookTest> planningData)
        {
            foreach (var connectionId in Subscribers.Keys)
            {
                await Clients.Client(connectionId).SendAsync("ReceivePlanningData", planningData);
            }
        }

        public static IReadOnlyCollection<string> GetSubscribers()
        {
            return Subscribers.Keys.ToList();
        }
    }
}

