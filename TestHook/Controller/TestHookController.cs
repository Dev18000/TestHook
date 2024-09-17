using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestHook.Data;
using Newtonsoft.Json;
using TestHook.Services;

namespace TestHook.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHookController : ControllerBase
    {
        private readonly IHookService _hookService;

        public TestHookController(IHookService hookService)
        {
            _hookService = hookService;
        }

        // Webhook endpoint that is triggered by external events and processes the received data.
        [HttpPost("TestWebHook")]
        public IActionResult TestWebHook([FromBody] IEnumerable<SimpleDataForHookTest> planningData)
        {
            // Logs the received data for debugging purposes.
            Console.WriteLine($"TestWebHook called with data: {JsonConvert.SerializeObject(planningData)}");

            // Notify subscribers asynchronously using the received data.
            _hookService.NotifyAsync(planningData);

            // Return HTTP 200 OK as a response to confirm the webhook was processed successfully.
            return Ok();
        }
    }
}
