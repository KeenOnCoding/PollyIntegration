namespace Client
{
    public interface ICorerouterIntegration
    {
        Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken token);
    }
}
