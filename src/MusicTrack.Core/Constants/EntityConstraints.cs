namespace MusicTrack.Core.Constants;

public static class EntityConstraints
{
    public const int ArtistNameMaxLength = 200;
    public const int EmailMaxLength = 256;
    public const int CountryMaxLength = 100;

    public const int TrackTitleMaxLength = 300;
    public const int IsrcLength = 12;
    public const int GenreMaxLength = 100;

    public const int DspNameMaxLength = 100;

    public const string IsrcPattern = "^[A-Z]{2}[A-Z0-9]{3}[0-9]{2}[0-9]{5}$";
}
