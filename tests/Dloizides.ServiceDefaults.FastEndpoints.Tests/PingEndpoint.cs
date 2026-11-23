using FastEndpoints;

namespace Dloizides.ServiceDefaults.FastEndpoints.Tests;

public enum PingStatus
{
    Ready,
}

public sealed record PingResponse(PingStatus Status);

public sealed class PingEndpoint : EndpointWithoutRequest<PingResponse>
{
    public const string Route = "/ping";

    public override void Configure()
    {
        Get(Route);
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return Send.OkAsync(new PingResponse(PingStatus.Ready), ct);
    }
}
