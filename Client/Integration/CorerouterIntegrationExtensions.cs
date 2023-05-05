using System;
using System.Net.Http;
using Polly;
using Polly.Registry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public static class CorerouterIntegrationExtensions
    {
        public static void AddCorerouterIntegration(this IServiceCollection services, IConfiguration configuration,
            IPolicyRegistry<string> registry)
        {
            services.AddServiceIntegration<ICorerouterIntegration, CorerouterIntegration, CorerouterConfiguration>(
                CorerouterIntegration.HttpClientKey, configuration);

            var cfg = configuration.GetSection(nameof(CorerouterConfiguration)).Get<CorerouterConfiguration>();

            var policy = Policy.Handle<HttpRequestException>()
                                .OrResult<HttpResponseMessage>(r => cfg.RetryStatusCodes.Contains((int)r.StatusCode))
                                .WaitAndRetryAsync(cfg.RetryCount, retryAttempt => TimeSpan.FromMilliseconds(
                                        Math.Round(Math.Pow(cfg.RetryBase, retryAttempt), cfg.RetryFractionalDigits, MidpointRounding.AwayFromZero)))
                                .WrapAsync(Policy.TimeoutAsync(TimeSpan.FromSeconds(cfg.RequestLifetime)));

            registry.Add(CorerouterIntegration.HttpClientKey, policy);
        }
    }
}
