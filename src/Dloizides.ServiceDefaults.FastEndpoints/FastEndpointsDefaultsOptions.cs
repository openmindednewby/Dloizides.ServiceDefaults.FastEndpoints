using ApiDocs.AspNetCore.Swagger;

namespace Dloizides.ServiceDefaults.FastEndpoints;

public sealed class FastEndpointsDefaultsOptions
{
    public const string DefaultRoutePrefix = "api/v1";

    public const string SwaggerPath = "/swagger";

    public string RoutePrefix { get; set; } = DefaultRoutePrefix;

    public Action<OAuth2PkceSwaggerOptions>? ConfigureSwagger { get; set; }
}
