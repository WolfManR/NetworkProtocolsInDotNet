using RootApiReference;

namespace FrontApi.Services;

public sealed class RootApiClient
{
    private readonly RootApiReference.RootApiClient _client;

    public RootApiClient(HttpClient client) => _client = new("http://localhost:5281", client);

    public async Task<ICollection<WeatherForecast>> GetWeather()
    {
        return await _client.GetWeatherForecastAsync();
    }
}