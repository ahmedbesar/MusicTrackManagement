using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Queries;

public sealed record GetAllArtistsQuery : IRequest<Result<IEnumerable<ArtistResponseDto>>>;
