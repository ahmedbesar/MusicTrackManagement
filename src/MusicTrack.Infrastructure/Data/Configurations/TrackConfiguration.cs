using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTrack.Core.Constants;
using MusicTrack.Core.Entities;

namespace MusicTrack.Infrastructure.Data.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Tracks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(EntityConstraints.TrackTitleMaxLength);

        builder.Property(t => t.Isrc)
            .IsRequired()
            .HasMaxLength(EntityConstraints.IsrcLength);

        builder.Property(t => t.Genre)
            .IsRequired()
            .HasMaxLength(EntityConstraints.GenreMaxLength);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(t => t.Isrc).IsUnique();
    }
}
