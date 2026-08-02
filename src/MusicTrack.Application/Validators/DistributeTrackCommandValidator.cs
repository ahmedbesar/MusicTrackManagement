using FluentValidation;
using MusicTrack.Application.Commands;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Validators;

public sealed class DistributeTrackCommandValidator : AbstractValidator<DistributeTrackCommand>
{
    public DistributeTrackCommandValidator(IDspRepository dspRepository)
    {
        RuleFor(x => x.TrackId)
            .NotEqual(Guid.Empty).WithMessage("TrackId is required");

        RuleFor(x => x.DspIds)
            .NotEmpty().WithMessage("At least one DspId is required")
            .MustAsync(async (dspIds, cancellationToken) =>
            {
                var distinctIds = dspIds.Distinct().ToList();
                var existingDsps = await dspRepository.GetByIdsAsync(distinctIds, cancellationToken);
                return existingDsps.Count() == distinctIds.Count;
            })
            .WithMessage("One or more DspIds do not exist")
            .When(x => x.DspIds is { Count: > 0 });
    }
}
