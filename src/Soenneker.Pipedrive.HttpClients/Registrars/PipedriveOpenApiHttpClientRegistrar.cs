using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Pipedrive.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Pipedrive.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class PipedriveOpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="PipedriveOpenApiHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddPipedriveOpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<IPipedriveOpenApiHttpClient, PipedriveOpenApiHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="PipedriveOpenApiHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddPipedriveOpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<IPipedriveOpenApiHttpClient, PipedriveOpenApiHttpClient>();

        return services;
    }
}
