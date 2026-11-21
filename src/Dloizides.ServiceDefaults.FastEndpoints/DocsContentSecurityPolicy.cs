namespace Dloizides.ServiceDefaults.FastEndpoints;

public static class DocsContentSecurityPolicy
{
    /// <summary>Builds the Swagger UI policy; the identity provider origin may receive the PKCE token request.</summary>
    public static string For(string? authority)
    {
        var connectSources = Uri.TryCreate(authority, UriKind.Absolute, out var authorityUri)
            ? $"'self' {authorityUri.GetLeftPart(UriPartial.Authority)}"
            : "'self'";
        return "default-src 'none'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; "
            + $"img-src 'self' data:; font-src 'self' data:; connect-src {connectSources}; base-uri 'none'; form-action 'none'";
    }
}
