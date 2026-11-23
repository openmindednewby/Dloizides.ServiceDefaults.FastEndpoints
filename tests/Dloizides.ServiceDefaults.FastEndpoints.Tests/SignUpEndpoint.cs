using FastEndpoints;
using FluentValidation;
using Validation.Defaults.Rules;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public sealed record SignUpRequest(string Username, string DisplayName);

public sealed class SignUpValidator : Validator<SignUpRequest>
{
    public const string DisplayNameRequired = "Display name is required.";

    public SignUpValidator()
    {
        RuleFor(request => request.Username).ValidUsername();
        RuleFor(request => request.DisplayName).NotEmpty().WithMessage(DisplayNameRequired);
    }
}

public sealed class SignUpEndpoint : Endpoint<SignUpRequest>
{
    public const string Route = "/sign-up";

    public override void Configure()
    {
        Post(Route);
        AllowAnonymous();
    }

    public override Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        return SendNoContentAsync(ct);
    }
}
