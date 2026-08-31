[![](https://img.shields.io/nuget/v/soenneker.pipedrive.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.pipedrive.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.pipedrive.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.pipedrive.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.pipedrive.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.pipedrive.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.pipedrive.httpclients/codeql.yml?style=for-the-badge&label=codeql)](https://github.com/soenneker/soenneker.pipedrive.httpclients/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Pipedrive.HttpClients

Provides cached `HttpClient` instances for authenticated Pipedrive API v2 requests, including multi-account use.

## Installation

```bash
dotnet add package Soenneker.Pipedrive.HttpClients
```

## Configuration

```json
{
  "Pipedrive": {
    "ApiKey": "your-oauth-access-token",
    "ClientBaseUrl": "https://api.pipedrive.com/api/v2/"
  }
}
```

`Pipedrive:ApiKey` is retained as the configuration key for compatibility, but its default bearer template expects an OAuth access token. A personal Pipedrive API token is not interchangeable; Pipedrive requires personal tokens in the `api_token` query parameter.

## Usage

```csharp
using Soenneker.Pipedrive.HttpClients.Abstract;
using Soenneker.Pipedrive.HttpClients.Registrars;

services.AddPipedriveOpenApiHttpClientAsSingleton();

IPipedriveOpenApiHttpClient pipedrive = serviceProvider
    .GetRequiredService<IPipedriveOpenApiHttpClient>();

HttpClient client = await pipedrive.Get(cancellationToken);
HttpResponseMessage response = await client.GetAsync("activities", cancellationToken);
response.EnsureSuccessStatusCode();
```

To work with multiple Pipedrive accounts, pass each account's OAuth access token explicitly:

```csharp
HttpClient accountClient = await pipedrive.Get(accessToken, cancellationToken);
```

Each token receives its own cached client. Disposing the provider removes every client that provider created without invalidating clients owned by other provider instances.
