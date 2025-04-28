public class ServerWeatherClient : IWeatherClient
{
    public Task<WeatherForecast[]> GetWeatherForecasts()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);

        string[] summaries = [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];


        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray());
    }
}