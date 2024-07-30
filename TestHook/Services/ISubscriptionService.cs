namespace TestHook.Services
{
    public interface ISubscriptionService
    {
        Task Subscribe(string url);
        Task Unsubscribe(string url);
    }
}
