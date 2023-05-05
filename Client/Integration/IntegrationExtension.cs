namespace Client
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class IntegrationExtension
    {
        public static void AddServiceIntegration<TInterfaceIntegration, TIntegration, TConfiguration>(this IServiceCollection services, string httpClientKeyName, IConfiguration cfg, Action<HttpClient, TConfiguration> configureHttpClient = null) where TInterfaceIntegration : class where TIntegration : class, TInterfaceIntegration where TConfiguration : class, IServiceConfiguration
        {
            Type typeFromHandle = typeof(TConfiguration);
            IConfigurationSection section = cfg.GetSection(typeFromHandle.Name);
            services.Configure<TConfiguration>(section);
            TConfiguration httpClientCfg = section.Get<TConfiguration>();
            IHttpClientBuilder builder = services.AddHttpClient<TInterfaceIntegration, TIntegration>(httpClientKeyName, delegate (HttpClient client)
            {
                configureHttpClient?.Invoke(client, httpClientCfg);
                if (httpClientCfg.RequestLifetime >= 1)
                {
                    client.Timeout = TimeSpan.FromSeconds(httpClientCfg.RequestLifetime);
                }
            });
            if (httpClientCfg.RequestLifetime > 0)
            {
                builder.SetHandlerLifetime(TimeSpan.FromSeconds(httpClientCfg.RequestLifetime));
            }

            if (httpClientCfg.PropagateHeaders)
            {
                builder.AddHttpClientHeaders();
            }

            //builder.AddAudit();
            services.AddScoped<TInterfaceIntegration, TIntegration>();
        }

        public static IHttpClientBuilder AddIntegrationHttpClient<TConfiguration>(this IServiceCollection services, string httpClientKey, TConfiguration config, Action<HttpClient, TConfiguration> configureHttpClient = null) where TConfiguration : class, IServiceConfiguration
        {
            IHttpClientBuilder httpClientBuilder = services.AddHttpClient(httpClientKey, delegate (HttpClient client)
            {
                configureHttpClient?.Invoke(client, config);
            });
            if (config.PropagateHeaders)
            {
                httpClientBuilder.AddHttpClientHeaders();
            }

            //httpClientBuilder.AddAudit();
            return httpClientBuilder;
        }
    }
}
