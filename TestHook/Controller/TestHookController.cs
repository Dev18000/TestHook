using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestHook.Data;
using Newtonsoft.Json;

namespace TestHook.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHookController : ControllerBase
    {
        private readonly IHubContext<PlanningHub> _hubContext;

        public TestHookController(IHubContext<PlanningHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("TestWebHook")]
        public async Task<IActionResult> TestWebHook([FromBody] IEnumerable<SimpleDataForHookTest> planningData)
        {
            var subscribers = PlanningHub.GetSubscribers();
            Console.WriteLine($"Received TestWebHook call with data: {JsonConvert.SerializeObject(planningData)}");
            foreach (var subscriber in subscribers)
            {
                Console.WriteLine($"Sending data to subscriber: {subscriber}");
                await _hubContext.Clients.Client(subscriber).SendAsync("ReceivePlanningData", planningData);
            }
            return Ok();
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody] SubscriptionRequest request)
        {
            Console.WriteLine($"Subscription request received for URL: {request.Url}");
            return Ok(new { status = "subscribed" });
        }

        [HttpPost("unsubscribe")]
        public IActionResult Unsubscribe([FromBody] SubscriptionRequest request)
        {
            Console.WriteLine($"Unsubscription request received for URL: {request.Url}");
            return Ok(new { status = "unsubscribed" });
        }
    }
}
