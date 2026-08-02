using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Queries;

public sealed record GetAllDspsQuery : IRequest<Result<IEnumerable<DspResponseDto>>>;
