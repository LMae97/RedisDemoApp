using Microsoft.AspNetCore.Mvc;
using RedisDemoApp.Models;
using RedisDemoApp.Services;

namespace RedisDemoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> GetAll() { 
            return await _weatherForecastService.GetForecastsAsync(DateTime.Now);
        }

    }
}