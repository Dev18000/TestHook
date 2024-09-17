using System.Text.Json;
using TestHook.Data;
using TestHook.Services;

public class HookService : IHookService
{
    // Event to store subscribers for hook updates
    private event Action<IEnumerable<SimpleDataForHookTest>> eventTest;

    // Notify all subscribers about new data asynchronously
    public async Task NotifyAsync(IEnumerable<SimpleDataForHookTest> message)
    {
        // Invoke the event, notifying all registered handlers (subscribers)
        eventTest?.Invoke(message);
    }

    // Register a handler (subscriber) to receive updates
    public void Register(Action<IEnumerable<SimpleDataForHookTest>> handler)
    {
        Console.WriteLine($"Register called with handler: {handler}");
        // Add the handler to the event
        eventTest += handler;
    }

    // Unregister a handler (subscriber) so it no longer receives updates
    public void UnRegister(Action<IEnumerable<SimpleDataForHookTest>> handler)
    {
        // Remove the handler from the event
        eventTest -= handler;
    }
}
