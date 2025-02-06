using Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackgrounTaskServiceExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public WeatherForecastController(IBackgroundTaskQueue backgroundTaskQueue)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
          

            var i = 0;
            while (i < 100)
            {
                i++;
                var current = i;
                _backgroundTaskQueue.EnqueueTask(async token =>
                {
                    await SomeMethod(current);
                });


            }

            return Ok("Task added to queue.");
        }

        [HttpGet("id")]
        public async Task SomeMethod(int i)
        {
            await Task.Delay(5000);
            Console.WriteLine($"Task Execute. {i}");
        }

    }

    public class SomeClass
    {
    }
}
