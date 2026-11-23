using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Validation.Defaults.Constants;

namespace Dloizides.ServiceDefaults.FastEndpoints;

internal static class ValidationProblemDetails
{
    public static readonly Action<ErrorOptions.ProblemDetailsConfig> Configure = problem =>
    {
        problem.AllowDuplicateErrors = true;
        problem.IndicateErrorCode = true;
        problem.IndicateErrorSeverity = false;
    };

    public static object Build(List<ValidationFailure> failures, HttpContext context, int statusCode) =>
        new global::FastEndpoints.ProblemDetails(WithSharedCodes(failures), context.Request.Path, context.TraceIdentifier, statusCode);

    private static List<ValidationFailure> WithSharedCodes(List<ValidationFailure> failures) =>
        failures.Select(failure => new ValidationFailure(failure.PropertyName, failure.ErrorMessage, failure.AttemptedValue)
        {
            ErrorCode = ValidationCodes.FromFluentValidation(failure.ErrorCode ?? string.Empty),
            Severity = failure.Severity,
            CustomState = failure.CustomState,
        }).ToList();
}
