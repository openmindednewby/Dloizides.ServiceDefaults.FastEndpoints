# Dloizides.ServiceDefaults.FastEndpoints

Add-on to Dloizides.ServiceDefaults: FastEndpoints with the house JSON settings and route prefix, plus the Keycloak OAuth2 PKCE Swagger UI.

```csharp
builder.AddDloizidesServiceDefaults(o => o.ServiceName = "MyService");
builder.AddDloizidesFastEndpoints();
var app = builder.Build();
app.UseDloizidesServiceDefaults();
app.UseDloizidesFastEndpoints();
```

- Endpoints map under `api/v1` (`o.RoutePrefix` changes it); JSON is camelCase, case-insensitive on input, enums as strings.
- `Swagger:OAuth:Authority` (falls back to `Jwt:Authority`), `Swagger:OAuth:ClientId` and `Swagger:OAuth:Scopes` drive the OAuth2 scheme and the Swagger UI PKCE client.
- `/swagger` responses carry the docs Content-Security-Policy (inline Swagger UI scripts, token request to the authority origin); Dloizides.SecurityHeaders keeps it and adds `frame-ancestors 'none'`.
