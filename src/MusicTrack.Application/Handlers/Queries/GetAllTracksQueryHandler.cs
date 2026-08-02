using FluentResults;
using MediatR;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Queries;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Queries;

public sealed class GetAllTracksQueryHandler : IRequestHandler<GetAllTracksQuery, Result<IEnumerable<TrackResponseDto>>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly TrackMapper _mapper;

    public GetAllTracksQueryHandler(ITrackRepository trackRepository, TrackMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TrackResponseDto>>> Handle(GetAllTracksQuery request, CancellationToken cancellationToken)
    {
        var tracks = await _trackRepository.GetAllAsync(request.ArtistId, request.Genre, request.Status, cancellationToken);
        return Result.Ok(_mapper.ToResponseListDto(tracks));
    }
}
