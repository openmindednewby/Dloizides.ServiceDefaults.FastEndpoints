using System.Net;
using Shouldly;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed class EndpointMappingTests
{
    [Fact]
    public async Task UseDloizidesFastEndpoints_WithDefaultOptions_MapsEndpointUnderApiV1WithCamelCaseEnumNames()
    {
        await using var app = await FastEndpointsHost.StartAsync(new Dictionary<string, string?>(StringComparer.Ordinal));

        var response = await app.GetAsync("/api/v1/ping");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.BodyAsync(app)).ShouldBe("{\"status\":\"Ready\"}");
    }

    [Fact]
    public async Task UseDloizidesFastEndpoints_WithCustomRoutePrefix_MapsEndpointUnderThatPrefix()
    {
        await using var app = await FastEndpointsHost.StartAsync(
            new Dictionary<string, string?>(StringComparer.Ordinal),
            o => o.RoutePrefix = "api/v2");

        var response = await app.GetAsync("/api/v2/ping");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UseDloizidesFastEndpoints_WithDefaultOptions_DoesNotMapEndpointWithoutPrefix()
    {
        await using var app = await FastEndpointsHost.StartAsync(new Dictionary<string, string?>(StringComparer.Ordinal));

        var response = await app.GetAsync("/ping");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
