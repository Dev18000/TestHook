namespace TestHook.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl = $""; // Base API URL for subscription

        // Constructor: Initializes the HTTP client factory
        public SubscriptionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Method to subscribe a URL to receive hook notifications
        public async Task Subscribe(string url)
        {
            Console.WriteLine($"Subscribing to URL: {url}");
            var httpClient = _httpClientFactory.CreateClient(); // Create an HTTP client
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/subscribe", new { Url = url }); // Send the subscription request
            response.EnsureSuccessStatusCode(); // Ensure the response indicates success
        }

        // Method to unsubscribe a URL from receiving hook notifications
        public async Task Unsubscribe(string url)
        {
            Console.WriteLine($"Unsubscribing from URL: {url}");
            var httpClient = _httpClientFactory.CreateClient(); // Create an HTTP client
            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/unsubscribe", new { Url = url }); // Send the unsubscription request
            response.EnsureSuccessStatusCode(); // Ensure the response indicates success
        }
    }
}
