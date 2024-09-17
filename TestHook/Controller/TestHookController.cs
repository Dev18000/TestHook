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
            Console.WriteLine($"TestWebHook called with data: {JsonConvert.SerializeObject(planningData)}");
            _hookService.NotifyAsync(planningData);
            return Ok();
        }
    }
}
