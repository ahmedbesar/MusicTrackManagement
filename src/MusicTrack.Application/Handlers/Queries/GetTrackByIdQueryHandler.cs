using FluentResults;
using MediatR;
using MusicTrack.Application.Errors;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Queries;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Queries;

public sealed class GetTrackByIdQueryHandler : IRequestHandler<GetTrackByIdQuery, Result<TrackDetailResponseDto>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly TrackMapper _mapper;

    public GetTrackByIdQueryHandler(ITrackRepository trackRepository, TrackMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    public async Task<Result<TrackDetailResponseDto>> Handle(GetTrackByIdQuery request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.Id, cancellationToken);

        if (track is null)
            return Result.Fail<TrackDetailResponseDto>(new NotFoundError($"Track with id '{request.Id}' was not found"));

        return Result.Ok(_mapper.ToDetailResponseDto(track));
    }
}
