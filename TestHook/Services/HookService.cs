using TestHook.Data;

namespace TestHook.Services
{
    public class HookService : IHookService
    {
        private event Action<IEnumerable<SimpleDataForHookTest>> eventTest;

        public void Notify(IEnumerable<SimpleDataForHookTest> message)
        {
            Console.WriteLine($"Notify called with message: {message}");
            eventTest?.Invoke(message);
        }

        public void Register(Action<IEnumerable<SimpleDataForHookTest>> handler)
        {
            Console.WriteLine($"Register called with handler: {handler}");
            eventTest += handler;
        }

        public void UnRegister(Action<IEnumerable<SimpleDataForHookTest>> handler)
        {
            Console.WriteLine($"UnRegister called with handler: {handler}");
            eventTest -= handler;
        }
    }
}
