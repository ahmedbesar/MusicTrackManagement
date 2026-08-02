using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Commands;

public sealed record CreateTrackCommand : IRequest<Result<TrackResponseDto>>
{
    public string Title { get; init; } = default!;
    public Guid ArtistId { get; init; }
    public string Isrc { get; init; } = default!;
    public DateOnly ReleaseDate { get; init; }
    public string Genre { get; init; } = default!;
}
