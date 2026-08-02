using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Queries;

public sealed record GetTrackByIdQuery(Guid Id) : IRequest<Result<TrackDetailResponseDto>>;
