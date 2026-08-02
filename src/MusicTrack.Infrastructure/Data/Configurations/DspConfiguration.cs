using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicTrack.Core.Constants;
using MusicTrack.Core.Entities;

namespace MusicTrack.Infrastructure.Data.Configurations;

public class DspConfiguration : IEntityTypeConfiguration<Dsp>
{
    public void Configure(EntityTypeBuilder<Dsp> builder)
    {
        builder.ToTable("Dsps");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(EntityConstraints.DspNameMaxLength);

        builder.HasIndex(d => d.Name).IsUnique();
    }
}
