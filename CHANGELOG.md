# Changelog

## 0.3.0

- Public `Config.UseDloizidesValidationProblemDetails()` (FORM-VALID-1 wave 2): a service that keeps its own `UseFastEndpoints` opts into the ProblemDetails validation body without the string-enum converter or the PKCE Swagger document. Built on FastEndpoints 7.0.1.

## 0.2.0

- `EnableValidationProblemDetails` (off by default, FORM-VALID-1 D2): validation 400s become RFC 9457 ProblemDetails with `errors[{name, reason, code}]`; FluentValidation built-in codes map to REQUIRED / TOO_SHORT / TOO_LONG / INVALID_FORMAT through DLoizides.Validation 1.3.0 `ValidationCodes.FromFluentValidation`; custom codes pass unchanged.

## 0.1.0

- First version: AddDloizidesFastEndpoints / UseDloizidesFastEndpoints with the api/v1 prefix, camelCase JSON and string enums; OAuth2 PKCE Swagger document and UI bound from Swagger:OAuth; docs Content-Security-Policy on /swagger (SVCDEF-1 pilot.2 item C).
