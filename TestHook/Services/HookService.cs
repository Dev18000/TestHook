using System.Text.Json;
using TestHook.Data;
using TestHook.Services;

public class HookService : IHookService
{
    private event Action<IEnumerable<SimpleDataForHookTest>> eventTest;

    public async Task NotifyAsync(IEnumerable<SimpleDataForHookTest> message)
    {
        eventTest?.Invoke(message);
    }

    public void Register(Action<IEnumerable<SimpleDataForHookTest>> handler)
    {
        Console.WriteLine($"Register called with handler: {handler}");
        eventTest += handler;
    }

    public void UnRegister(Action<IEnumerable<SimpleDataForHookTest>> handler)
    {
        eventTest -= handler;
    }
}
