using Shouldly;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed class PkceConfigTests
{
    private const string Authority = "https://identity.example.test/realms/demo";
    private const string ClientId = "swagger-client";

    [Fact]
    public async Task UseDloizidesFastEndpoints_WithSwaggerOAuthSection_BindsPkceClientIntoSwaggerUi()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Swagger:OAuth:Authority"] = Authority,
            ["Swagger:OAuth:ClientId"] = ClientId,
        };
        await using var app = await FastEndpointsHost.StartAsync(settings);

        var page = await (await app.GetAsync(FastEndpointsHost.SwaggerUiPath)).BodyAsync(app);

        page.ShouldContain("clientId: \"swagger-client\"");
        page.ShouldContain("usePkceWithAuthorizationCodeGrant: true");
    }

    [Fact]
    public async Task UseDloizidesFastEndpoints_WithSwaggerOAuthSection_BindsAuthorityIntoOAuthScheme()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Swagger:OAuth:Authority"] = Authority,
            ["Swagger:OAuth:ClientId"] = ClientId,
        };
        await using var app = await FastEndpointsHost.StartAsync(settings);

        var document = await (await app.GetAsync(FastEndpointsHost.SwaggerJsonPath)).BodyAsync(app);

        document.ShouldContain("https://identity.example.test/realms/demo/protocol/openid-connect/auth");
        document.ShouldContain("https://identity.example.test/realms/demo/protocol/openid-connect/token");
    }

    [Fact]
    public async Task UseDloizidesFastEndpoints_WithoutSwaggerAuthority_FallsBackToJwtAuthority()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal) { ["Jwt:Authority"] = Authority };
        await using var app = await FastEndpointsHost.StartAsync(settings);

        var document = await (await app.GetAsync(FastEndpointsHost.SwaggerJsonPath)).BodyAsync(app);

        document.ShouldContain("https://identity.example.test/realms/demo/protocol/openid-connect/auth");
    }
}
