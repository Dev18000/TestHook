using TestHook.Data;

namespace TestHook.Services
{
    public interface IHookService 
    {
        void Register(Action<IEnumerable<SimpleDataForHookTest>> handler);
        void UnRegister(Action<IEnumerable<SimpleDataForHookTest>> handler);
        Task NotifyAsync(IEnumerable<SimpleDataForHookTest> message);
    }
}
