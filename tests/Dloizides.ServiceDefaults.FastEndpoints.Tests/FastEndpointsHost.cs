using Dloizides.SecurityHeaders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

internal static class FastEndpointsHost
{
    public const string BaseAddress = "https://localhost";
    public const string SwaggerUiPath = "/swagger/index.html";
    public const string SwaggerJsonPath = "/swagger/v1/swagger.json";
    public const string CspHeader = "Content-Security-Policy";

    public static async Task<WebApplication> StartAsync(
        IDictionary<string, string?> settings,
        Action<FastEndpointsDefaultsOptions>? configure = null)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = Environments.Production });
        builder.WebHost.UseTestServer();
        builder.Configuration.AddInMemoryCollection(settings);
        builder.Services.AddDloizidesSecurityHeaders();
        builder.AddDloizidesFastEndpoints(configure);
        var app = builder.Build();
        app.UseDloizidesSecurityHeaders();
        app.UseDloizidesFastEndpoints();
        await app.StartAsync(app.Lifetime.ApplicationStopping);
        return app;
    }

    public static Task<HttpResponseMessage> GetAsync(this WebApplication app, string path)
    {
        var client = app.GetTestClient();
        client.BaseAddress = new Uri(BaseAddress);
        return client.GetAsync(path, app.Lifetime.ApplicationStopping);
    }

    public static async Task<HttpResponseMessage> PostJsonAsync(this WebApplication app, string path, string json)
    {
        var client = app.GetTestClient();
        client.BaseAddress = new Uri(BaseAddress);
        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        return await client.PostAsync(path, content, app.Lifetime.ApplicationStopping);
    }

    public static Task<string> BodyAsync(this HttpResponseMessage response, WebApplication app)
    {
        return response.Content.ReadAsStringAsync(app.Lifetime.ApplicationStopping);
    }

    public static string[] ContentSecurityPolicies(this HttpResponseMessage response)
    {
        return response.Headers.TryGetValues(CspHeader, out var values) ? values.ToArray() : [];
    }
}
