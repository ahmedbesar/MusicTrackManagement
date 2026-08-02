using FluentResults;
using MediatR;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Errors;
using MusicTrack.Application.Mappers;
using MusicTrack.Application.Responses;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Enums;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Handlers.Commands;

public sealed class DistributeTrackCommandHandler : IRequestHandler<DistributeTrackCommand, Result<TrackDetailResponseDto>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly TrackMapper _mapper;

    public DistributeTrackCommandHandler(ITrackRepository trackRepository, TrackMapper mapper)
    {
        _trackRepository = trackRepository;
        _mapper = mapper;
    }

    public async Task<Result<TrackDetailResponseDto>> Handle(DistributeTrackCommand request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.TrackId, cancellationToken);

        if (track is null)
            return Result.Fail<TrackDetailResponseDto>(new NotFoundError($"Track with id '{request.TrackId}' was not found"));

        foreach (var dspId in request.DspIds.Distinct())
        {
            var alreadyDistributed = await _trackRepository.DistributionExistsAsync(track.Id, dspId, cancellationToken);
            if (alreadyDistributed)
                continue;

            var distribution = TrackDistribution.Create(track.Id, dspId);
            await _trackRepository.AddDistributionAsync(distribution, cancellationToken);
        }

        track.UpdateStatus(TrackStatus.Submitted);
        await _trackRepository.SaveChangesAsync(cancellationToken);

        var updatedTrack = await _trackRepository.GetByIdAsync(track.Id, cancellationToken);
        return Result.Ok(_mapper.ToDetailResponseDto(updatedTrack!));
    }
}
