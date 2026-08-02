using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Commands;

public sealed record DistributeTrackCommand : IRequest<Result<TrackDetailResponseDto>>
{
    public Guid TrackId { get; init; }
    public IReadOnlyCollection<Guid> DspIds { get; init; } = new List<Guid>();
}
