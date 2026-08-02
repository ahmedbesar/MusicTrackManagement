using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Enums;

namespace MusicTrack.Application.Commands;

public sealed record UpdateTrackStatusCommand : IRequest<Result<TrackDetailResponseDto>>
{
    public Guid TrackId { get; init; }
    public TrackStatus Status { get; init; }
}
