using RedisDemoApp.Models;

namespace RedisDemoApp.Services
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>> GetForecastsAsync(DateTime startDate);
    }
}