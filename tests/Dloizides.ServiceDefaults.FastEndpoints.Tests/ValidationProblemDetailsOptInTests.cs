using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed class ValidationProblemDetailsOptInTests
{
    private const string TraceId = "trace-1";
    private const string SignUpPath = "/api/v1/sign-up";
    private const string BadSignUp = """{"username":"bad name!","displayName":""}""";

    [Fact]
    public async Task UseDloizidesValidationProblemDetails_WithInvalidBody_ReturnsProblemDetailsByteForByte()
    {
        await using var app = await StartPlainHostAsync();

        using var response = await app.PostJsonAsync(SignUpPath, BadSignUp);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            """{"type":"https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1","title":"Bad Request","status":400,"instance":"/api/v1/sign-up","traceId":"trace-1","errors":[{"name":"username","reason":"username may only contain letters, digits, underscore, dot, or dash.","code":"USERNAME_CHARSET"},{"name":"displayName","reason":"Display name is required.","code":"REQUIRED"}]}""",
            await response.BodyAsync(app));
    }

    [Fact]
    public async Task UseDloizidesValidationProblemDetails_WithEnumResponse_KeepsEnumAsNumber()
    {
        await using var app = await StartPlainHostAsync();

        using var response = await app.GetAsync("/api/v1" + PingEndpoint.Route);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("""{"status":0}""", await response.BodyAsync(app));
    }

    private static async Task<WebApplication> StartPlainHostAsync()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Production });
        builder.WebHost.UseTestServer();
        builder.Services.AddFastEndpoints();
        var app = builder.Build();
        app.Use((context, next) =>
        {
            context.TraceIdentifier = TraceId;
            return next(context);
        });
        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = FastEndpointsDefaultsOptions.DefaultRoutePrefix;
            config.UseDloizidesValidationProblemDetails();
        });
        await app.StartAsync(app.Lifetime.ApplicationStopping);
        return app;
    }
}
