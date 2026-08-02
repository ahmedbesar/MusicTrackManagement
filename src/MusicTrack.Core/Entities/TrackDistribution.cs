using MusicTrack.Core.Common;
using MusicTrack.Core.Enums;

namespace MusicTrack.Core.Entities;

public class TrackDistribution : BaseEntity
{
    public Guid TrackId { get; private set; }
    public Guid DspId { get; private set; }
    public DateTime SubmittedAt { get; private set; }
    public DistributionStatus Status { get; private set; }

    public Track? Track { get; private set; }
    public Dsp? Dsp { get; private set; }

    private TrackDistribution()
    {
    }

    public static TrackDistribution Create(Guid trackId, Guid dspId)
    {
        return new TrackDistribution
        {
            TrackId = trackId,
            DspId = dspId,
            SubmittedAt = DateTime.UtcNow,
            Status = DistributionStatus.Pending
        };
    }

    public void UpdateStatus(DistributionStatus status)
    {
        Status = status;
    }
}
