# Music Track Management

A Track Management API and single-page UI for a fictional music distribution company. Artists and their tracks are managed through a .NET Web API, and tracks can be submitted to DSPs (Spotify, Apple Music, YouTube) for distribution. An Angular SPA lets you browse tracks and inspect their DSP distribution status.

## Solution layout

```text
MusicTrackManagement.slnx
src/
  MusicTrack.Api/             Controllers, Program.cs, JWT auth, Swagger, ResultExtensions
  MusicTrack.Application/     Commands, Queries, Handlers, Validators, Behaviors, Mappers, Responses, Constants
  MusicTrack.Core/            Entities, enums, repository interfaces
  MusicTrack.Infrastructure/  EF Core DbContext, entity configurations, migrations, repositories, seed data
music-track-ui/                Angular 21 SPA
DECISIONS.md                   AI usage notes for this task
```

Architecture: Clean Architecture + CQRS. `Api → Application → Core ← Infrastructure`. Requests flow through a MediatR pipeline (`UnhandledExceptionBehavior` → `LoggingBehavior` → `ValidationBehavior`), handlers talk to repository interfaces defined in Core, and results are returned as `FluentResults.Result`/`Result<T>` which the API maps to HTTP responses (200 / 400 / 404).

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB (ships with Visual Studio, or install "SQL Server Express LocalDB" separately)
- Node.js 20+ and npm
- Angular CLI 21 (`npm install -g @angular/cli`) — optional, `npx` also works

## Backend — running the API

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --project src/MusicTrack.Api
```

The API listens on `http://localhost:5002` (see `src/MusicTrack.Api/Properties/launchSettings.json`). On startup it automatically:

1. Applies any pending EF Core migrations (`context.Database.MigrateAsync()`).
2. Seeds the database if it's empty (4 artists, 10 tracks across multiple genres/statuses, 3 DSPs, and sample distributions) — see `src/MusicTrack.Infrastructure/Data/MusicTrackDbSeed.cs`.

Swagger UI is available at `http://localhost:5002/swagger` in the Development environment, including a bearer-token "Authorize" button for the JWT-protected endpoints.

### Database & migrations

The connection string lives in `src/MusicTrack.Api/appsettings.json` under `ConnectionStrings:MusicTrackConnection` and points at `(localdb)\MSSQLLocalDB`. Migrations live in `src/MusicTrack.Infrastructure/Migrations`.

To add a new migration or update the database manually (from the repo root):

```bash
dotnet tool install --global dotnet-ef   # once, if you don't already have it
dotnet ef migrations add <Name> --project src/MusicTrack.Infrastructure --startup-project src/MusicTrack.Api
dotnet ef database update --project src/MusicTrack.Infrastructure --startup-project src/MusicTrack.Api
```

Design-time migration tooling uses `MusicTrackDbContextFactory` in the Infrastructure project, so `dotnet ef` commands work even without running the API host.

### Obtaining a JWT

`POST /api/tracks/{id}/distribute` and `PATCH /api/tracks/{id}/status` are `[Authorize]`-protected. Get a token from the demo login endpoint:

```bash
curl -X POST http://localhost:5002/api/auth/token \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"admin\",\"password\":\"ChangeMe123!\"}"
```

This returns `{ "accessToken": "...", "expiresAtUtc": "..." }`. Send the token on subsequent requests as `Authorization: Bearer <accessToken>` (Swagger's "Authorize" button does this for you). Demo credentials and the JWT signing key are in `appsettings.json` for convenience — see [`DECISIONS.md`](DECISIONS.md) for the security trade-offs of that choice.

### API endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/token` | — | Issue a demo JWT |
| POST | `/api/artists` | — | Create an artist |
| GET | `/api/artists` | — | List all artists |
| POST | `/api/tracks` | — | Create a track for an artist |
| GET | `/api/tracks?artistId=&genre=&status=` | — | List tracks with optional filters |
| GET | `/api/tracks/{id}` | — | Get track details, including DSP distribution statuses |
| POST | `/api/tracks/{id}/distribute` | JWT | Submit a track to one or more DSPs |
| PATCH | `/api/tracks/{id}/status` | JWT | Update a track's status (Draft/Submitted/Distributed) |
| GET | `/api/dsps` | — | List DSPs (used by the UI) |

## Frontend — running the Angular SPA

```bash
cd music-track-ui
npm install
npm start   # ng serve
```

The app runs at `http://localhost:4200` and calls the API at `http://localhost:4200/api` → configured via `src/environments/environment.ts` (`apiUrl: 'http://localhost:5002/api'`). The API's CORS policy allows `http://localhost:4200` (see `Program.cs`).

### Features

- **Track List** (`/tracks`) — every track with artist name, genre, and status, plus a status filter dropdown.
- **Track Detail** (`/tracks/:id`) — full track info and a table of DSP distribution statuses.
- **Sign in** (`/login`) — signs in against `/api/auth/token`, stores the JWT, and an HTTP interceptor attaches it as a bearer token to outgoing requests. Use `admin` / `ChangeMe123!`.

### Tests & build

```bash
npm test -- --watch=false
npm run build
```

## Running both together

1. Start the API: `dotnet run --project src/MusicTrack.Api` (port 5002).
2. Start the UI: `cd music-track-ui && npm start` (port 4200).
3. Open `http://localhost:4200`, browse tracks, sign in, and try `distribute`/status updates via Swagger or a REST client with the bearer token.

## Commit history

This repository was built as a series of small, focused commits (one feature per commit) rather than a single squashed change — see `git log` for the full sequence from solution scaffolding through to the UI.
