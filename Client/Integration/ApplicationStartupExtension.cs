namespace Client
{
    using System;
    using System.Globalization;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;

    public static class ApplicationStartupExtension
    {
        public static IHttpClientBuilder AddHttpClientHeaders(this IHttpClientBuilder builder)
        {
            return builder.AddHeaderPropagation(options => options.Headers.Add("sample-header"));
        }
    }
}
