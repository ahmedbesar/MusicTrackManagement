using MusicTrack.Core.Common;

namespace MusicTrack.Core.Entities;

public class Dsp : BaseEntity
{
    public string Name { get; private set; } = default!;

    public ICollection<TrackDistribution> Distributions { get; private set; } = new List<TrackDistribution>();

    private Dsp()
    {
    }

    public static Dsp Create(string name)
    {
        return new Dsp
        {
            Name = name
        };
    }
}
