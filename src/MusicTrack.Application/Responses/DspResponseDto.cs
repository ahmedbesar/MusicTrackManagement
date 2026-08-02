namespace MusicTrack.Application.Responses;

public sealed record DspResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
}
