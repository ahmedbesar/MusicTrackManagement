# DECISIONS.md — AI Usage Notes

This project was built end-to-end in Cursor with an AI coding agent (Claude, driven via Cursor) doing the vast majority of the typing, under my direction and review. This file documents what came from the AI, what I steered or changed, the security issues I found, and one concrete thing the AI got wrong.

## 1. What did AI generate, and what did I write/modify myself?

**AI-generated (the bulk of the code):**

- The whole Clean Architecture skeleton — `MusicTrack.Api/Application/Core/Infrastructure` projects, project references, and the `.slnx` wiring.
- Domain entities (`Artist`, `Track`, `Dsp`, `TrackDistribution`) with static factory methods and private setters, plus the `TrackStatus`/`DistributionStatus` enums.
- The full CQRS slice per feature: commands/queries, MediatR handlers, FluentValidation validators, Mapperly mappers, and response DTOs.
- EF Core wiring: `MusicTrackDbContext`, `IEntityTypeConfiguration<T>` classes, the `InitialCreate` migration, `IDesignTimeDbContextFactory`, and the seed data (`MusicTrackDbSeed`).
- The MediatR pipeline behaviors (`UnhandledExceptionBehavior`, `LoggingBehavior`, `ValidationBehavior`) and `ResultExtensions.ToHttpResponse()`.
- JWT auth: `JwtSettings`, `TokenService`, `AuthController`, and the Swagger bearer-token configuration.
- The entire Angular SPA: models, `TrackService`/`AuthService`, the auth interceptor, the login form, and the Track List / Track Detail components including their templates and styles.

**What I steered/decided myself (not just accepted defaults):**

- I gave the AI an explicit reference project (`D:\work\MicroServices\Ecommerce\Ecommerce`) and told it to mirror that project's modern services (Catalog/Basket), not its older `Ordering` service, for layering, `Result`/`FluentResults` usage, and the MediatR `ValidationBehavior` pattern — instead of the custom `ResultDto<T>` I had originally sketched out. That reconciliation (rules file → real reference repo → merged conventions) was a decision I made and had the AI document in `.cursor/rules/coding-standards.mdc`.
- I explicitly chose "simple JWT" over the reference project's full OpenIddict setup, and "local db" (SQL Server LocalDB) over containerized SQL Server, to keep the task's scope reasonable — the AI's first instinct was to lean on the heavier setup from the reference repo.
- I reviewed and corrected the `TrackStatusTransitionPolicy` rules (which status transitions are legal) rather than accepting whatever the AI initially proposed, and I reviewed every Swagger/API response manually via PowerShell test scripts before moving to the next slice.
- I made the call on commit granularity (one commit per feature, matching the todo list) and rejected the idea of a single large commit.
- I picked the actual demo credentials/signing key values and moved the demo password out of the AI's control into a value I control and can document (see below).

I did not write raw C#/TypeScript line-by-line, but I read every generated file, ran the build/tests after each slice, and pushed back on anything that didn't match the reference repo's conventions or introduced risk.

## 2. What security issues did I find (or the AI introduce)? How did I handle them?

- **Secrets committed in `appsettings.json`.** The AI put the JWT signing key and the demo user's password hash directly in `appsettings.json`. That's fine for a takehome demo but is a real anti-pattern for production. I kept it as-is but named the key `dev-only-signing-key-do-not-use-in-production-...` and call this out explicitly in the README/here so it's not mistaken for a production-ready setup. In a real deployment, these values belong in user-secrets/environment variables/a secrets manager, never in source control.
- **Unsalted SHA-256 for the demo password hash.** The AI initially just hashed the password with `SHA256.HashData` and compared bytes. That's acceptable only because there is a single hardcoded demo account with no real user registration flow — for anything beyond a demo I would require a proper password hasher (`PBKDF2`/`bcrypt`/`Argon2`) with a per-user salt. I did keep the AI's use of `CryptographicOperations.FixedTimeEquals` for the hash comparison (avoiding a timing side-channel), which was a good instinct on its part.
- **Overly permissive CORS.** The generated CORS policy used `.AllowAnyMethod().AllowAnyHeader()`. I scoped the *origin* to `http://localhost:4200` only (not `AllowAnyOrigin`), which is the important control for this app, but I flagged the any-method/any-header breadth as something to tighten (to only the verbs/headers actually used) before this went anywhere near production.
- **No rate limiting / lockout on `POST /api/auth/token`.** The login endpoint has no throttling, so it's brute-forceable. Out of scope for a 5-day take-home, but worth calling out — I did not add it, and wouldn't ship this endpoint publicly without it.
- **Swagger exposed only in Development** (`app.Environment.IsDevelopment()`), which the AI got right by default and I didn't need to change — good, since Swagger + a bearer scheme is not something you want exposed in production.
- **Enum values and IDs sent as opaque strings/GUIDs, no sequential integer IDs.** The AI's default of `Guid` primary keys avoids ID-enumeration attacks that come for free with auto-increment integer IDs — I kept this rather than "simplifying" to `int`.

## 3. One thing the AI got wrong that I had to fix

While wiring up Swagger's JWT "Authorize" button, the AI initially generated code using `Microsoft.OpenApi.Models.OpenApiInfo` and the classic `OpenApiSecurityScheme.Reference` + `AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = ... })` pattern that's correct for Swashbuckle.AspNetCore v6/v7. That code didn't compile: the project actually resolved `Swashbuckle.AspNetCore` v10, which pulls in `Microsoft.OpenApi` v2.x — a major version where `OpenApiInfo` and friends moved out of the `Microsoft.OpenApi.Models` namespace into `Microsoft.OpenApi` directly, and `OpenApiSecurityScheme.Reference` was removed in favor of a `document`-aware `OpenApiSecuritySchemeReference` type passed into a lambda-based `AddSecurityRequirement` overload.

This was wrong because the AI was pattern-matching on the most common (older, still widely-documented) Swashbuckle API shape rather than the version actually resolved by the package manager for this project. It compiled the *idea* correctly (bearer scheme + global security requirement) but not the concrete API surface for the version in use. I had to diagnose the build errors, look at what changed between Microsoft.OpenApi v1 and v2 (via web search, since this is a genuinely recent breaking change), and rewrite the Swagger configuration in `Program.cs` to use `using Microsoft.OpenApi;` and:

```csharp
options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
{
    [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
});
```

The broader lesson: when a library has recently shipped a breaking major version, an AI trained on a snapshot of the ecosystem will confidently reproduce the *old* API unless you make it check the actual installed package version and compile the code to verify — which is exactly what caught this here.
