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

        [Inject]
        public ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        public IHookService HookService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("https://localhost:0000/planninghub")) // your hook url adress
                .Build();

            hubConnection.On<IEnumerable<SimpleDataForHookTest>>("ReceiveUpdate", (data) =>
            {
                updates = data.ToList();
                InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("Subscribe", "https://localhost:0000/api/TestHook/TestWebHook"); // your blazor adress

            HookService.Register(ReceivePlanningData);
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
                Console.WriteLine("Not send data from hook");
            }
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.SendAsync("Unsubscribe");
            HookService.UnRegister(ReceivePlanningData);
            await hubConnection.DisposeAsync();
        }
    }
}
