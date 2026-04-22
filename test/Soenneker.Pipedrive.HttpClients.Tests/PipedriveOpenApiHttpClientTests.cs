using Soenneker.Pipedrive.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Pipedrive.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class PipedriveOpenApiHttpClientTests : HostedUnitTest
{
    private readonly IPipedriveOpenApiHttpClient _httpclient;

    public PipedriveOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<IPipedriveOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
