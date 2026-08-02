using MusicTrack.Core.Common;
using MusicTrack.Core.Enums;

namespace MusicTrack.Core.Entities;

public class Track : BaseEntity
{
    public string Title { get; private set; } = default!;
    public Guid ArtistId { get; private set; }
    public string Isrc { get; private set; } = default!;
    public DateOnly ReleaseDate { get; private set; }
    public string Genre { get; private set; } = default!;
    public TrackStatus Status { get; private set; }

    public Artist? Artist { get; private set; }
    public ICollection<TrackDistribution> Distributions { get; private set; } = new List<TrackDistribution>();

    private Track()
    {
    }

    public static Track Create(string title, Guid artistId, string isrc, DateOnly releaseDate, string genre)
    {
        return new Track
        {
            Title = title,
            ArtistId = artistId,
            Isrc = isrc,
            ReleaseDate = releaseDate,
            Genre = genre,
            Status = TrackStatus.Draft
        };
    }

    public void UpdateStatus(TrackStatus status)
    {
        Status = status;
    }
}
