using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICorerouterIntegration _corerouterIntegration;
        public WeatherForecastController(ICorerouterIntegration corerouterIntegration, ILogger<WeatherForecastController> logger)
        {
            _corerouterIntegration = corerouterIntegration;
            _logger = logger;
        }

        [HttpGet(Name = "Values")]
        public async Task<IActionResult> Values()
        {
            var source = new CancellationTokenSource();
            var result = await _corerouterIntegration.GetAsync(source.Token);

            return Ok(result);
        }
    }
}