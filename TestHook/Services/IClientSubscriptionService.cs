namespace TestHook.Services
{
    public interface IClientSubscriptionService
    {
        Task Subscribe(string url);
        Task Unsubscribe(string url);
    }
}
