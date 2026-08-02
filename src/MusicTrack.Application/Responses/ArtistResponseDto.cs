namespace MusicTrack.Application.Responses;

public sealed record ArtistResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Country { get; init; } = default!;
}
