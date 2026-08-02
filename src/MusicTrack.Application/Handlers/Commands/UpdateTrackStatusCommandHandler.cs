using FluentResults;
using MediatR;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Constants;
using MusicTrack.Application.Errors;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Commands;

public sealed class UpdateTrackStatusCommandHandler : IRequestHandler<UpdateTrackStatusCommand, Result<TrackDetailResponseDto>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly TrackMapper _mapper;

    public UpdateTrackStatusCommandHandler(ITrackRepository trackRepository, TrackMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    public async Task<Result<TrackDetailResponseDto>> Handle(UpdateTrackStatusCommand request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.TrackId, cancellationToken);

        if (track is null)
            return Result.Fail<TrackDetailResponseDto>(new NotFoundError($"Track with id '{request.TrackId}' was not found"));

        if (!TrackStatusTransitionPolicy.IsValidTransition(track.Status, request.Status))
            return Result.Fail<TrackDetailResponseDto>(
                $"Cannot transition track status from '{track.Status}' to '{request.Status}'");

        track.UpdateStatus(request.Status);
        await _trackRepository.SaveChangesAsync(cancellationToken);

        return Result.Ok(_mapper.ToDetailResponseDto(track));
    }
}
