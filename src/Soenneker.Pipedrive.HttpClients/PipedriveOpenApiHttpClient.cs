using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Hashing.XxHash;
using Soenneker.Pipedrive.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Pipedrive.HttpClients;

///<inheritdoc cref="IPipedriveOpenApiHttpClient"/>
public sealed class PipedriveOpenApiHttpClient : IPipedriveOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;
    private readonly Uri _baseUrl;
    private readonly string _authHeaderName;
    private readonly string _authHeaderValueTemplate;
    private readonly ConcurrentDictionary<string, byte> _clientIds = new();

    private const string _prodBaseUrl = "https://api.pipedrive.com/api/v2/";

    public PipedriveOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _configuration = config;
        _baseUrl = new Uri(config["Pipedrive:ClientBaseUrl"] ?? _prodBaseUrl);
        _authHeaderName = config["Pipedrive:AuthHeaderName"] ?? "Authorization";
        _authHeaderValueTemplate = config["Pipedrive:AuthHeaderValueTemplate"] ?? "Bearer {token}";
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return Get(_configuration.GetValueStrict<string>("Pipedrive:ApiKey"), cancellationToken);
    }

    public ValueTask<HttpClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        string clientId = GetClientId(apiKey);
        _clientIds.TryAdd(clientId, 0);

        return _httpClientCache.Get(clientId,
            (apiKey, baseUrl: _baseUrl, authHeaderName: _authHeaderName, authHeaderValueTemplate: _authHeaderValueTemplate), static state =>
            {
                string authHeaderValue = state.authHeaderValueTemplate.Replace("{token}", state.apiKey, StringComparison.Ordinal);

                return new HttpClientOptions
                {
                    BaseAddress = state.baseUrl,
                    DefaultRequestHeaders = new Dictionary<string, string>
                    {
                        {state.authHeaderName, authHeaderValue},
                    }
                };
            }, cancellationToken);
    }

    private string GetClientId(string apiKey)
    {
        string value = string.Concat(apiKey, "\0", _baseUrl, "\0", _authHeaderName, "\0", _authHeaderValueTemplate);

        return $"{nameof(PipedriveOpenApiHttpClient)}:{XxHash3Util.Hash(value)}";
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            _httpClientCache.RemoveSync(clientId);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            await _httpClientCache.Remove(clientId);
        }
    }
}
