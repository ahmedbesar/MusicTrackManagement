using FluentValidation;
using MusicTrack.Application.Commands;

namespace MusicTrack.Application.Validators;

public sealed class UpdateTrackStatusCommandValidator : AbstractValidator<UpdateTrackStatusCommand>
{
    public UpdateTrackStatusCommandValidator()
    {
        RuleFor(x => x.TrackId)
            .NotEqual(Guid.Empty).WithMessage("TrackId is required");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid track status");
    }
}
