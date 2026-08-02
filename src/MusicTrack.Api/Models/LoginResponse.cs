namespace MusicTrack.Api.Models;

public sealed record LoginResponse
{
    public string AccessToken { get; init; } = default!;
    public DateTime ExpiresAtUtc { get; init; }
}
