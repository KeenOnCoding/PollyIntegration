namespace Client
{
    public interface ICorerouterIntegration
    {
        Task<WeatherForecast> GetAsync(CancellationToken token);
    }
}
