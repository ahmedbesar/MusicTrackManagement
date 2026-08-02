using FluentResults;
using MediatR;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Commands;

public sealed class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, Result<TrackResponseDto>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly TrackMapper _mapper;

    public CreateTrackCommandHandler(ITrackRepository trackRepository, TrackMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    public async Task<Result<TrackResponseDto>> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {
        var track = Track.Create(request.Title, request.ArtistId, request.Isrc, request.ReleaseDate, request.Genre);
        var createdTrack = await _trackRepository.AddAsync(track, cancellationToken);

        var trackWithArtist = await _trackRepository.GetByIdAsync(createdTrack.Id, cancellationToken);

        return Result.Ok(_mapper.ToResponseDto(trackWithArtist!));
    }
}
