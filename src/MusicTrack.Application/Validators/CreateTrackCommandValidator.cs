using FluentValidation;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Constants;
using MusicTrack.Core.Interfaces;

namespace MusicTrack.Application.Validators;

public sealed class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
{
    public CreateTrackCommandValidator(IArtistRepository artistRepository, ITrackRepository trackRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(ValidationConstants.TrackTitleMaxLength)
            .WithMessage($"Title must not exceed {ValidationConstants.TrackTitleMaxLength} characters");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .MaximumLength(ValidationConstants.GenreMaxLength)
            .WithMessage($"Genre must not exceed {ValidationConstants.GenreMaxLength} characters");

        RuleFor(x => x.ReleaseDate)
            .NotEqual(default(DateOnly)).WithMessage("ReleaseDate is required");

        RuleFor(x => x.Isrc)
            .NotEmpty().WithMessage("Isrc is required")
            .Length(ValidationConstants.IsrcLength)
            .WithMessage($"Isrc must be exactly {ValidationConstants.IsrcLength} characters")
            .Matches(ValidationConstants.IsrcPattern)
            .WithMessage("Isrc must match the ISRC format, e.g. USRC17607839")
            .MustAsync(async (isrc, cancellationToken) => !await trackRepository.IsrcExistsAsync(isrc, cancellationToken))
            .WithMessage("Isrc must be unique")
            .When(x => !string.IsNullOrWhiteSpace(x.Isrc));

        RuleFor(x => x.ArtistId)
            .NotEqual(Guid.Empty).WithMessage("ArtistId is required")
            .MustAsync(async (artistId, cancellationToken) => await artistRepository.ExistsAsync(artistId, cancellationToken))
            .WithMessage("Artist does not exist")
            .When(x => x.ArtistId != Guid.Empty);
    }
}
