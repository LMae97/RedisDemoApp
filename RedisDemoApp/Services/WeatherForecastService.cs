using RedisDemoApp.Infrastructure;
using RedisDemoApp.Models;

namespace RedisDemoApp.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ICacheContext _distributedCacheContext;

    public WeatherForecastService(ICacheContext distributedCacheContext)
    {
        _distributedCacheContext = distributedCacheContext;
    }

    public async Task<List<WeatherForecast>> GetForecastsAsync(DateTime startDate)
    {
        string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmm");

        var forecasts = await _distributedCacheContext.GetRecordAsync<List<WeatherForecast>>(currentDate);

        if (forecasts is default(List<WeatherForecast>))
        {
            forecasts = (await GetDummyForecasts(DateTime.Now)).ToList();

            await _distributedCacheContext.SetRecordAsync(currentDate, forecasts); //Salvo i dati in cache
        }

        return forecasts;
    }

    private static async Task<WeatherForecast[]> GetDummyForecasts(DateTime startDate)
    {
        var rng = new Random();
        await Task.Delay(1500);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToArray();
    }
}
