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

## 3. One thing the AI got wrong that I had to fix
