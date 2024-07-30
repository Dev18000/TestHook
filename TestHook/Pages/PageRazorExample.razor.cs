using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Text;
using TestHook.Data;
using TestHook.Services;

namespace TestHook.Pages
{
    public partial class PageRazorExample : IDisposable
    {
        [Inject]
        public IHookService HookService { get; set; }
        [Inject]
        public ISubscriptionService SubscriptionService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            MyPropertyTestHook = new List<SimpleDataForHookTest>()
            {
                new SimpleDataForHookTest(){ MyProperty = 123 }
            };

            Console.WriteLine("OnInitializedAsync called");
            Console.WriteLine($"HookService instance ID: {HookService.GetHashCode()}");
            await SubscriptionService.Subscribe($"https://localhost:7052/api/TestHook/TestWebHook");
            HookService.Register(ReceivePlanningData);
            Console.WriteLine("Handler registered");
        }

       

        IEnumerable<SimpleDataForHookTest> MyPropertyTestHook { get; set; }
        private void ReceivePlanningData(IEnumerable<SimpleDataForHookTest> planningListDto)
        {

            // i need debug here, for update data from my hook
            var dataHook = planningListDto;
            if (dataHook != null && dataHook.Any())
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

        public void Dispose()
        {
            Console.WriteLine("Dispose called");
            HookService.UnRegister(ReceivePlanningData);
            SubscriptionService.Unsubscribe($"https://localhost:7052/api/TestHook/TestWebHook").GetAwaiter().GetResult(); // todo find solution send ip dinamic 
            Console.WriteLine("Handler unregistered and unsubscribed");
        }
    }
}
