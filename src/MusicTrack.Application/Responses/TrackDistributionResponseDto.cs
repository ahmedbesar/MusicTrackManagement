using MusicTrack.Core.Enums;

namespace MusicTrack.Application.Responses;

public sealed record TrackDistributionResponseDto
{
    public Guid Id { get; init; }
    public Guid DspId { get; init; }
    public string DspName { get; init; } = default!;
    public DateTime SubmittedAt { get; init; }
    public DistributionStatus Status { get; init; }
}
