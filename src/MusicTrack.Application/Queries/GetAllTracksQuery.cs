using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Enums;

namespace MusicTrack.Application.Queries;

public sealed record GetAllTracksQuery : IRequest<Result<IEnumerable<TrackResponseDto>>>
{
    public Guid? ArtistId { get; init; }
    public string? Genre { get; init; }
    public TrackStatus? Status { get; init; }
}
