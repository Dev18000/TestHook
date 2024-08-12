using Microsoft.AspNetCore.SignalR;

namespace TestHook.Data
{
    public class UpdateHub : Hub
    {
        public async Task SendUpdate(IEnumerable<SimpleDataForHookTest> data)
        {
            await Clients.All.SendAsync("ReceiveUpdate", data);
        }
    }
}
