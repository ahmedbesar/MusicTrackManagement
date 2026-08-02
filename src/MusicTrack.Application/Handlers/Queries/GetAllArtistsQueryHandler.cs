using FluentResults;
using MediatR;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Queries;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Queries;

public sealed class GetAllArtistsQueryHandler : IRequestHandler<GetAllArtistsQuery, Result<IEnumerable<ArtistResponseDto>>>
{
    private readonly IArtistRepository _artistRepository;
    private readonly ArtistMapper _mapper;

    public GetAllArtistsQueryHandler(IArtistRepository artistRepository, ArtistMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ArtistResponseDto>>> Handle(GetAllArtistsQuery request, CancellationToken cancellationToken)
    {
        var artists = await _artistRepository.GetAllAsync(cancellationToken);
        return Result.Ok(_mapper.ToResponseListDto(artists));
    }
}
