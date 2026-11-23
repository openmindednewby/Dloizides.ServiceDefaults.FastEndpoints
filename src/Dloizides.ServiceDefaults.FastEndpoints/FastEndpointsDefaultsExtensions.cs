using System.Text.Json;
using System.Text.Json.Serialization;
using ApiDocs.AspNetCore.Extensions;
using ApiDocs.AspNetCore.Swagger;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;

namespace Dloizides.ServiceDefaults.FastEndpoints;

public static class FastEndpointsDefaultsExtensions
{
    /// <summary>Adds FastEndpoints and the OAuth2 PKCE Swagger document.</summary>
    public static WebApplicationBuilder AddDloizidesFastEndpoints(
        this WebApplicationBuilder builder,
        Action<FastEndpointsDefaultsOptions>? configure = null)
    {
        var options = new FastEndpointsDefaultsOptions();
        configure?.Invoke(options);
        var swagger = OAuth2PkceSwaggerOptions.FromConfiguration(builder.Configuration);
        options.ConfigureSwagger?.Invoke(swagger);
        builder.Services.AddSingleton(new FastEndpointsDefaultsSettings(
            options.RoutePrefix,
            swagger.ClientId,
            swagger.Title,
            DocsContentSecurityPolicy.For(swagger.Authority),
            options.EnableValidationProblemDetails));
        builder.Services.AddFastEndpoints().AddOAuth2PkceSwaggerDocument(builder.Configuration, options.ConfigureSwagger);
        return builder;
    }

    /// <summary>Maps the endpoints and serves the Swagger UI under the docs Content-Security-Policy.</summary>
    public static WebApplication UseDloizidesFastEndpoints(this WebApplication app)
    {
        var settings = app.Services.GetRequiredService<FastEndpointsDefaultsSettings>();
        app.UseWhen(
            context => context.Request.Path.StartsWithSegments(FastEndpointsDefaultsOptions.SwaggerPath, StringComparison.OrdinalIgnoreCase),
            docs => docs.Use((context, next) =>
            {
                context.Response.Headers.ContentSecurityPolicy = settings.DocsContentSecurityPolicy;
                return next(context);
            }));
        app.UseFastEndpoints(config =>
            {
                config.Endpoints.RoutePrefix = settings.RoutePrefix;
                config.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.Serializer.Options.PropertyNameCaseInsensitive = true;
                config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
                if (settings.EnableValidationProblemDetails)
                {
                    config.UseDloizidesValidationProblemDetails();
                }
            })
            .UseSwaggerGen(uiConfig: ui => ui.OAuth2Client = new OAuth2ClientSettings
            {
                ClientId = settings.SwaggerClientId ?? string.Empty,
                AppName = settings.SwaggerTitle ?? string.Empty,
                UsePkceWithAuthorizationCodeGrant = true,
            });
        return app;
    }
}
