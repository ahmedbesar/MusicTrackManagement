using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTrack.Core.Constants;
using MusicTrack.Core.Entities;

namespace MusicTrack.Infrastructure.Data.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(EntityConstraints.ArtistNameMaxLength);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(EntityConstraints.EmailMaxLength);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(EntityConstraints.CountryMaxLength);

        builder.HasMany(a => a.Tracks)
            .WithOne(t => t.Artist)
            .HasForeignKey(t => t.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
