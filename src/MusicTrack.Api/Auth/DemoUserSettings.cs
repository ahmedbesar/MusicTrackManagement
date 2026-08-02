namespace MusicTrack.Api.Auth;

/// <summary>
/// Demo-only credential source. There is no user store in this task, so a single
/// account is configured for JWT issuance. The password is never stored in plain
/// text: only its SHA-256 hash lives in configuration.
/// </summary>
public sealed class DemoUserSettings
{
    public const string SectionName = "DemoUser";

    public string Username { get; init; } = default!;
    public string PasswordHash { get; init; } = default!;
}
