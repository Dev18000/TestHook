using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Text;
using TestHook.Data;
using TestHook.Services;

namespace TestHook.Pages
{
    public partial class PageRazorExample : IAsyncDisposable
    {
        [Inject]
        public IClientSubscriptionService SubscriptionService { get; set; }
        private HubConnection? hubConnection;
        private string subscriptionUrl = $"https://localhost:7052/api/TestHook/TestWebHook";
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        private HttpClient HttpClient => HttpClientFactory.CreateClient("ApiClient");
        [Inject]
        public HttpClient HttpHook { get; set; }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                try
                {
                    await hubConnection.InvokeAsync("Unsubscribe");
                    Console.WriteLine("Successfully unsubscribed from the hub.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during unsubscription: {ex.Message}");
                }

                await hubConnection.DisposeAsync();
            }

            await UnsubscribeAsync(subscriptionUrl);
        }

        private async Task SubscribeToEvent()
        {
            var url = "https://localhost:7006/api/Event/subscribe";
            var payload = new { Url = "https://localhost:7052/planning" };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successfully subscribed to the API.");
                    Console.WriteLine(responseContent);
                }
                else
                {
                    Console.WriteLine($"Error during subscription: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during subscription: {ex.Message}");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            MyPropertyTestHook = new List<SimpleDataForHookTest>()
            {
                new SimpleDataForHookTest(){ MyProperty = 123 }
            };

            await SubscribeToEvent();

            hubConnection = new HubConnectionBuilder()
         .WithUrl(Navigation.ToAbsoluteUri("/planninghub"))
         .Build();

            hubConnection.On<IEnumerable<SimpleDataForHookTest>>("ReceivePlanningData", (data) =>
            {
                Console.WriteLine($"Received planning data: {JsonConvert.SerializeObject(data)}");
                ReceivePlanningData(data);
                InvokeAsync(StateHasChanged);
            });

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(Random.Shared.Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            await hubConnection.StartAsync();

            try
            {
                await hubConnection.InvokeAsync("Subscribe");
                Console.WriteLine("Successfully subscribed to the hub.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during subscription: {ex.Message}");
            }

            subscriptionUrl = $"https://localhost:7006/api/Event/subscribe";
            Console.WriteLine($"Subscribing to API: {subscriptionUrl}");

            Console.WriteLine($"About to call SubscribeAsync with URL: {subscriptionUrl}");

            await SubscribeAsync(subscriptionUrl);

            Console.WriteLine($"Finished calling SubscribeAsync with URL: {subscriptionUrl}");
        }

        private async Task SubscribeAsync(string url)
        {
            Console.WriteLine($"Attempting to subscribe to {url}");
            try
            {
                await SubscriptionService.Subscribe(url);
                Console.WriteLine("Successfully subscribed to API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during subscription: {ex.Message}");
            }
        }

        private async Task UnsubscribeAsync(string url)
        {
            Console.WriteLine($"Attempting to unsubscribe from {url}");
            try
            {
                await SubscriptionService.Unsubscribe(url);
                Console.WriteLine("Successfully unsubscribed from API.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during unsubscription: {ex.Message}");
            }
        }

        IEnumerable<SimpleDataForHookTest> MyPropertyTestHook { get; set; }
        private void ReceivePlanningData(IEnumerable<SimpleDataForHookTest> planningListDto)
        {

            // i need debug here, for update data from my hook
            var dataHook = planningListDto;
            if (dataHook != null && dataHook.Any())
            {
                var list = MyPropertyTestHook.ToList();

                Console.WriteLine("Received planning data.");
                InvokeAsync(StateHasChanged);
            }
            else
            {
                Console.WriteLine("Not send data from hook");
            }
        }
    }
}
