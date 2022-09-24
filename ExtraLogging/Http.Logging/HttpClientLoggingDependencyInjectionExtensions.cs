using Http.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClientLoggingDependencyInjectionExtensions
{
    public static IServiceCollection AddHttpClientLogging(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, HttpClientMessageHandlerBuilderFilter>());
        return services;
    }
}