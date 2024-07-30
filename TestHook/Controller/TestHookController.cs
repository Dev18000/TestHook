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

        [HttpPost("TestWebHook")]
        public IActionResult TestWebHook([FromBody] IEnumerable<SimpleDataForHookTest> planningData)
        {
            Console.WriteLine($"TestWebHook called with data: {planningData}");
            Console.WriteLine($"HookService instance ID: {_hookService.GetHashCode()}");
            _hookService.Notify(planningData);
            return Ok();
        }
    }
}
