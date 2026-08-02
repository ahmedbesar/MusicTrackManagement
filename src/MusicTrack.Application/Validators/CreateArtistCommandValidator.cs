using FluentValidation;
using MusicTrack.Application.Commands;
using MusicTrack.Application.Constants;

namespace MusicTrack.Application.Validators;

public sealed class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    public CreateArtistCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(ValidationConstants.ArtistNameMaxLength)
            .WithMessage($"Name must not exceed {ValidationConstants.ArtistNameMaxLength} characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(ValidationConstants.EmailMaxLength)
            .WithMessage($"Email must not exceed {ValidationConstants.EmailMaxLength} characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(ValidationConstants.CountryMaxLength)
            .WithMessage($"Country must not exceed {ValidationConstants.CountryMaxLength} characters");
    }
}
