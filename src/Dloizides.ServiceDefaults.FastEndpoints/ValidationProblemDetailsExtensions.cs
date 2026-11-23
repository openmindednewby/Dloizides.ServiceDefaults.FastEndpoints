using FastEndpoints;

namespace Dloizides.ServiceDefaults.FastEndpoints;

public static class ValidationProblemDetailsExtensions
{
    /// <summary>Turns validation 400s into RFC 9457 ProblemDetails with shared error codes; changes nothing else.</summary>
    public static Config UseDloizidesValidationProblemDetails(this Config config)
    {
        config.Errors.UseProblemDetails(ValidationProblemDetails.Configure);
        config.Errors.ResponseBuilder = ValidationProblemDetails.Build;
        return config;
    }
}
