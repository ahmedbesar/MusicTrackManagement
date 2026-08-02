using MusicTrack.Core.Common;

namespace MusicTrack.Core.Entities;

public class Artist : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Country { get; private set; } = default!;

    public ICollection<Track> Tracks { get; private set; } = new List<Track>();

    private Artist()
    {
    }

    public static Artist Create(string name, string email, string country)
    {
        return new Artist
        {
            Name = name,
            Email = email,
            Country = country
        };
    }
}
