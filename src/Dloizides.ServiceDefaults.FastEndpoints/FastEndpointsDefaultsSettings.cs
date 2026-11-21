namespace Dloizides.ServiceDefaults.FastEndpoints;

internal sealed record FastEndpointsDefaultsSettings(
    string RoutePrefix,
    string? SwaggerClientId,
    string? SwaggerTitle,
    string DocsContentSecurityPolicy);
