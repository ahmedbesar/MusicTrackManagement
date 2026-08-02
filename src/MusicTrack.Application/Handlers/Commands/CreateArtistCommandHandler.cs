using FluentResults;
using MediatR;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Commands;

public sealed class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, Result<ArtistResponseDto>>
{
    private readonly IArtistRepository _artistRepository;
    private readonly ArtistMapper _mapper;

    public CreateArtistCommandHandler(IArtistRepository artistRepository, ArtistMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }

    public async Task<Result<ArtistResponseDto>> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var artist = Artist.Create(request.Name, request.Email, request.Country);
        var createdArtist = await _artistRepository.AddAsync(artist, cancellationToken);

        return Result.Ok(_mapper.ToResponseDto(createdArtist));
    }
}
