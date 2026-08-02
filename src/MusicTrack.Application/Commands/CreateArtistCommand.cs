using FluentResults;
using MediatR;
using MusicTrack.Application.Responses;

namespace MusicTrack.Application.Commands;

public sealed record CreateArtistCommand : IRequest<Result<ArtistResponseDto>>
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Country { get; init; } = default!;
}
