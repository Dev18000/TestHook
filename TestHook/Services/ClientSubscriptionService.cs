
namespace TestHook.Services
{
    public class ClientSubscriptionService : IClientSubscriptionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl = $"https://localhost:7052/api/Event";

        public ClientSubscriptionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Subscribe(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/subscribe", new { Url = url });
            response.EnsureSuccessStatusCode();
        }

        public async Task Unsubscribe(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/unsubscribe", new { Url = url });
            response.EnsureSuccessStatusCode();
        }
    }
}
