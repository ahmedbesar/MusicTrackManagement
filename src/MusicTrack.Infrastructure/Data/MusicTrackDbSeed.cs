using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicTrack.Core.Entities;
using MusicTrack.Core.Enums;

namespace MusicTrack.Infrastructure.Data;

public static class MusicTrackDbSeed
{
    public static async Task SeedAsync(MusicTrackDbContext context, ILogger logger)
    {
        if (await context.Artists.AnyAsync())
        {
            logger.LogInformation("Database already seeded, skipping");
            return;
        }

        var ariaMonroe = Artist.Create("Aria Monroe", "aria.monroe@example.com", "United States");
        var theNightOwls = Artist.Create("The Night Owls", "contact@nightowls.example.com", "United Kingdom");
        var kenjiSato = Artist.Create("Kenji Sato", "kenji.sato@example.com", "Japan");
        var lolaFuentes = Artist.Create("Lola Fuentes", "lola.fuentes@example.com", "Spain");

        await context.Artists.AddRangeAsync(ariaMonroe, theNightOwls, kenjiSato, lolaFuentes);

        var spotify = Dsp.Create("Spotify");
        var appleMusic = Dsp.Create("Apple Music");
        var youTube = Dsp.Create("YouTube");

        await context.Dsps.AddRangeAsync(spotify, appleMusic, youTube);

        var neonSkyline = CreateTrack("Neon Skyline", ariaMonroe.Id, "USRC12400001", new DateOnly(2024, 3, 15), "Pop", TrackStatus.Distributed);
        var wildfireHeart = CreateTrack("Wildfire Heart", ariaMonroe.Id, "USRC12400002", new DateOnly(2024, 6, 1), "Pop", TrackStatus.Submitted);
        var backroadBlues = CreateTrack("Backroad Blues", ariaMonroe.Id, "USRC12400009", new DateOnly(2022, 9, 12), "Country", TrackStatus.Submitted);

        var staticBloom = CreateTrack("Static Bloom", theNightOwls.Id, "GBUM72500003", new DateOnly(2025, 1, 20), "Rock", TrackStatus.Draft);
        var concreteJungle = CreateTrack("Concrete Jungle", theNightOwls.Id, "GBUM72500004", new DateOnly(2023, 11, 5), "Rock", TrackStatus.Distributed);
        var silentStatic = CreateTrack("Silent Static", theNightOwls.Id, "GBUM72500010", new DateOnly(2024, 12, 1), "Rock", TrackStatus.Distributed);

        var midnightRamen = CreateTrack("Midnight Ramen", kenjiSato.Id, "JPUM72500005", new DateOnly(2025, 2, 14), "Electronic", TrackStatus.Submitted);
        var cherryBlossomDrift = CreateTrack("Cherry Blossom Drift", kenjiSato.Id, "JPUM72500006", new DateOnly(2024, 4, 10), "Electronic", TrackStatus.Draft);

        var fuegoLento = CreateTrack("Fuego Lento", lolaFuentes.Id, "ESUM72500007", new DateOnly(2023, 8, 22), "Latin", TrackStatus.Distributed);
        var lunaLlena = CreateTrack("Luna Llena", lolaFuentes.Id, "ESUM72500008", new DateOnly(2025, 5, 30), "Latin", TrackStatus.Draft);

        await context.Tracks.AddRangeAsync(
            neonSkyline, wildfireHeart, backroadBlues,
            staticBloom, concreteJungle, silentStatic,
            midnightRamen, cherryBlossomDrift,
            fuegoLento, lunaLlena);

        await context.TrackDistributions.AddRangeAsync(
            CreateDistribution(neonSkyline.Id, spotify.Id, DistributionStatus.Live),
            CreateDistribution(neonSkyline.Id, appleMusic.Id, DistributionStatus.Live),
            CreateDistribution(wildfireHeart.Id, spotify.Id, DistributionStatus.Pending),
            CreateDistribution(backroadBlues.Id, youTube.Id, DistributionStatus.Pending),
            CreateDistribution(concreteJungle.Id, spotify.Id, DistributionStatus.Live),
            CreateDistribution(concreteJungle.Id, youTube.Id, DistributionStatus.Live),
            CreateDistribution(silentStatic.Id, spotify.Id, DistributionStatus.Live),
            CreateDistribution(midnightRamen.Id, appleMusic.Id, DistributionStatus.Pending),
            CreateDistribution(fuegoLento.Id, spotify.Id, DistributionStatus.Live),
            CreateDistribution(fuegoLento.Id, appleMusic.Id, DistributionStatus.Live),
            CreateDistribution(fuegoLento.Id, youTube.Id, DistributionStatus.Rejected));

        await context.SaveChangesAsync();

        logger.LogInformation("Seeded database with {ArtistCount} artists, {TrackCount} tracks, and {DspCount} DSPs",
            4, 10, 3);
    }

    private static Track CreateTrack(string title, Guid artistId, string isrc, DateOnly releaseDate, string genre, TrackStatus status)
    {
        var track = Track.Create(title, artistId, isrc, releaseDate, genre);
        track.UpdateStatus(status);
        return track;
    }

    private static TrackDistribution CreateDistribution(Guid trackId, Guid dspId, DistributionStatus status)
    {
        var distribution = TrackDistribution.Create(trackId, dspId);
        distribution.UpdateStatus(status);
        return distribution;
    }
}
