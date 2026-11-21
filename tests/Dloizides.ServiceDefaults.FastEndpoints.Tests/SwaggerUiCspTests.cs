using System.Net;
using Shouldly;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed class SwaggerUiCspTests
{
    private const string Authority = "https://identity.example.test/realms/demo";

    [Fact]
    public async Task UseDloizidesFastEndpoints_WhenSwaggerUiRequested_ServesItWithDocsCspAndFrameAncestorsNone()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal) { ["Swagger:OAuth:Authority"] = Authority };
        await using var app = await FastEndpointsHost.StartAsync(settings);

        var response = await app.GetAsync(FastEndpointsHost.SwaggerUiPath);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.ContentSecurityPolicies().ShouldBe([
            "default-src 'none'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self' data:; connect-src 'self' https://identity.example.test; base-uri 'none'; form-action 'none'",
            "frame-ancestors 'none'",
        ]);
    }

    [Fact]
    public async Task UseDloizidesFastEndpoints_WhenApiEndpointRequested_KeepsSecurityHeadersDefaultCsp()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal) { ["Swagger:OAuth:Authority"] = Authority };
        await using var app = await FastEndpointsHost.StartAsync(settings);

        var response = await app.GetAsync("/api/v1/ping");

        response.ContentSecurityPolicies().ShouldBe([
            "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'",
        ]);
    }
}
