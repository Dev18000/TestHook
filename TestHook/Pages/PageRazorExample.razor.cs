using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHook.Data;
using TestHook.Services;

namespace TestHook.Pages
{
    public partial class PageRazorExample : IAsyncDisposable
    {
        private List<SimpleDataForHookTest> updates;
        private HubConnection hubConnection;
        private bool isConnected;

        [Inject]
        public ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        public IHookService HookService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("https://localhost:0000/planninghub")) // your hook url adress
                .Build();

            // Handle received updates
            hubConnection.On<IEnumerable<SimpleDataForHookTest>>("ReceiveUpdate", (data) =>
            {
                updates = data.ToList();
                InvokeAsync(StateHasChanged);
            });

            // Handle connection closed event
            hubConnection.Closed += async (error) =>
            {
                isConnected = false;
                Console.WriteLine($"Connection closed. Error: {error?.Message}");
                await Task.Delay(new Random().Next(0, 5) * 1000); // Wait before retrying
                await ConnectWithRetryAsync(); // Try to reconnect
            };

            await ConnectWithRetryAsync();

            HookService.Register(ReceivePlanningData);
        }

        private async Task ConnectWithRetryAsync()
        {
            try
            {
                await hubConnection.StartAsync();
                isConnected = true;
                Console.WriteLine("Connected to the hub.");

                await hubConnection.SendAsync("Subscribe", "https://localhost:7052/api/TestHook/TestWebHook"); // your Blazor address
            }
            catch (Exception ex)
            {
                isConnected = false;
                Console.WriteLine($"Failed to connect: {ex.Message}");
            }
        }

        IEnumerable<SimpleDataForHookTest> MyPropertyTestHook { get; set; }

        private void ReceivePlanningData(IEnumerable<SimpleDataForHookTest> planningListDto)
        {
            if (planningListDto != null && planningListDto.Any())
            {
                MyPropertyTestHook = planningListDto;
                Console.WriteLine("Received planning data.");
                InvokeAsync(StateHasChanged);
            }
            else
            {
                Console.WriteLine("No data received from hook");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (isConnected)
            {
                await hubConnection.SendAsync("Unsubscribe");
            }
            HookService.UnRegister(ReceivePlanningData);
            await hubConnection.DisposeAsync();
        }
    }
}
