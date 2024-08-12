namespace TestHook.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl = $"https://localhost:7006/api/Event";

        public SubscriptionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Subscribe(string url)
        {
            Console.WriteLine($"Subscribing to URL: {url}");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/subscribe", new { Url = url });
            response.EnsureSuccessStatusCode();
        }

        public async Task Unsubscribe(string url)
        {
            Console.WriteLine($"Unsubscribing from URL: {url}");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/unsubscribe", new { Url = url });
            response.EnsureSuccessStatusCode();
        }
    }
}
