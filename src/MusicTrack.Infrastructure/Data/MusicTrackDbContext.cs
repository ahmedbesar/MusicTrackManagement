using Microsoft.EntityFrameworkCore;
using MusicTrack.Core.Entities;

namespace MusicTrack.Infrastructure.Data;

public class MusicTrackDbContext : DbContext
{
    public MusicTrackDbContext(DbContextOptions<MusicTrackDbContext> options) : base(options)
    {
    }

    public DbSet<Artist> Artists => Set<Artist>();

    public DbSet<Track> Tracks => Set<Track>();

    public DbSet<Dsp> Dsps => Set<Dsp>();

    public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicTrackDbContext).Assembly);
    }
}
