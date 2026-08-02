using MusicTrack.Core.Enums;

namespace MusicTrack.Application.Constants;

public static class TrackStatusTransitionPolicy
{
    private static readonly Dictionary<TrackStatus, TrackStatus[]> AllowedTransitions = new()
    {
        [TrackStatus.Draft] = [TrackStatus.Submitted],
        [TrackStatus.Submitted] = [TrackStatus.Draft, TrackStatus.Distributed],
        [TrackStatus.Distributed] = [TrackStatus.Submitted]
    };

    public static bool IsValidTransition(TrackStatus from, TrackStatus to)
    {
        return AllowedTransitions.TryGetValue(from, out var allowedTargets) && allowedTargets.Contains(to);
    }
}
