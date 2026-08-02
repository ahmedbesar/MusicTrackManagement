using MusicTrack.Core.Enums;

namespace MusicTrack.Application.Responses;

public sealed record TrackResponseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public Guid ArtistId { get; init; }
    public string ArtistName { get; init; } = default!;
    public string Isrc { get; init; } = default!;
    public DateOnly ReleaseDate { get; init; }
    public string Genre { get; init; } = default!;
    public TrackStatus Status { get; init; }
}
