using Soenneker.Pipedrive.HttpClients.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Pipedrive.HttpClients.Tests;

[Collection("Collection")]
public sealed class PipedriveOpenApiHttpClientTests : FixturedUnitTest
{
    private readonly IPipedriveOpenApiHttpClient _httpclient;

    public PipedriveOpenApiHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<IPipedriveOpenApiHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
