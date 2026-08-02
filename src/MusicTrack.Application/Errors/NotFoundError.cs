using FluentResults;

namespace MusicTrack.Application.Errors;

public sealed class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}
