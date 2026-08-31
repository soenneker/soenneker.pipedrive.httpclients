using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Pipedrive.HttpClients.Abstract;

/// <summary>
/// Provides authenticated HTTP clients for Pipedrive API v2.
/// </summary>
public interface IPipedriveOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a client configured with the access token in <c>Pipedrive:ApiKey</c>.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured HTTP client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a client configured for a specific Pipedrive OAuth access token.
    /// </summary>
    /// <param name="apiKey">The Pipedrive OAuth access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the configured client.</returns>
    ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default);
}
