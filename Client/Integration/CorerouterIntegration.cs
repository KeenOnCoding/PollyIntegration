namespace Client
{
    using System;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Polly;
    using Polly.Registry;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class CorerouterIntegration: ICorerouterIntegration
    {
        internal const string HttpClientKey = "corerouter_integration";

        private readonly CorerouterConfiguration _cfg;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPolicyRegistry<string> _policyRegistry;

        public CorerouterIntegration(IHttpClientFactory factory, IPolicyRegistry<string> registry, IOptions<CorerouterConfiguration> opt)
        {
            _cfg = opt.Value;
            _httpClientFactory = factory;
            _policyRegistry = registry;
        }
        public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken token)
        {
            var url = $"http://localhost:5204/WeatherForecast";
            var response = await SendRequestAsync(url, HttpMethod.Get, null, token);

            return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(response); ;
        }
        private async Task<string> SendRequestAsync(string url,HttpMethod method,HttpContent content,CancellationToken token,bool isNeedValidationError = false)
        {
            using var client = _httpClientFactory.CreateClient(HttpClientKey);

            if (_cfg.DefaultHeaders != null && _cfg.DefaultHeaders.Count > 0)
            {
                foreach (var (headerName, headerValue) in _cfg.DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(headerName, headerValue);
                }
            }

            var policy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(HttpClientKey);

            try
            {
                using var request = new HttpRequestMessage { Method = method, Content = content, RequestUri = new Uri(url) };
                using var response = await policy.ExecuteAsync(t => client.SendAsync(request, t), token);

                var responseContent = await response.Content.ReadAsStringAsync(token);

                if (response.IsSuccessStatusCode)
                {
                    return responseContent;
                }

                if (response.StatusCode == HttpStatusCode.UnprocessableEntity && isNeedValidationError)
                {
                    throw new Exception();
                }

                throw new Exception();
            }
            catch (TaskCanceledException taskCanceledException)
            {
                throw new Exception();
            }
        }
    }
}
