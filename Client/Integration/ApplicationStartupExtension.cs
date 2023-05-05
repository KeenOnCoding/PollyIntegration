namespace Client
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationStartupExtension
    {
        public static IHttpClientBuilder AddHttpClientHeaders(this IHttpClientBuilder builder)
        {
            return builder.AddHeaderPropagation();
        }
    }
}
