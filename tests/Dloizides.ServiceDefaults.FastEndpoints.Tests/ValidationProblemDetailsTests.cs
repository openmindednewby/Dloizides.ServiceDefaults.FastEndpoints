using System.Net;
using System.Text.Json;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed class ValidationProblemDetailsTests
{
    private const string SignUpPath = "/api/v1/sign-up";
    private const string BadSignUp = """{"username":"bad name!","displayName":""}""";

    private static readonly Dictionary<string, string?> Settings = new(StringComparer.Ordinal);

    [Fact]
    public async Task Post_WithProblemDetailsEnabled_ReturnsErrorsWithSharedCodes()
    {
        await using var app = await FastEndpointsHost.StartAsync(Settings, options => options.EnableValidationProblemDetails = true);

        using var response = await app.PostJsonAsync(SignUpPath, BadSignUp);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.BodyAsync(app));
        Assert.Equal(
            """[{"name":"username","reason":"username may only contain letters, digits, underscore, dot, or dash.","code":"USERNAME_CHARSET"},{"name":"displayName","reason":"Display name is required.","code":"REQUIRED"}]""",
            body.RootElement.GetProperty("errors").GetRawText());
    }

    [Fact]
    public async Task Post_WithProblemDetailsDefault_ReturnsFastEndpointsErrorMap()
    {
        await using var app = await FastEndpointsHost.StartAsync(Settings);

        using var response = await app.PostJsonAsync(SignUpPath, BadSignUp);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.BodyAsync(app));
        Assert.Equal(
            """{"username":["username may only contain letters, digits, underscore, dot, or dash."],"displayName":["Display name is required."]}""",
            body.RootElement.GetProperty("errors").GetRawText());
    }
}
